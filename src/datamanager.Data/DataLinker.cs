using System;
using datamanager.Entities;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace datamanager.Data
{
	public class DataLinker
	{
		public DataManagerSettings Settings;
		public DataChecker Checker;
		public DataReader Reader;
		public DataSaver Saver;
		public DataUpdater Updater;
		public EntityLinker Linker;

		public DataLinker (DataManagerSettings settings, DataReader reader, DataSaver saver, DataUpdater updater, DataChecker checker, EntityLinker linker)
		{
			Settings = settings;
			Linker = linker;
			Reader = reader;
			Saver = saver;
			Checker = checker;
		}

		public virtual void SaveLinkedEntities(BaseEntity entity)
		{
			if (Settings.IsVerbose)
				Console.WriteLine ("Saving all entities linked to '"  + entity.GetType().Name + "'.");
			
			foreach (var property in entity.GetType().GetProperties()) {
				if (Linker.IsLinkProperty (entity, property)) {
					SaveLinkedEntities (entity, property);
				}
			}
		}

		public virtual void SaveLinkedEntities(BaseEntity entity, PropertyInfo property)
		{
			var linkedEntities = Linker.GetLinkedEntities (entity, property);

			foreach (var e in linkedEntities) {
				if (!Checker.Exists (e)) {
					Saver.Save(entity, false, false); // Save without committing links otherwise it causes a loop
				}
			}
		}


		public virtual void UpdateLinkedEntities(BaseEntity entity)
		{
			if (Settings.IsVerbose)
				Console.WriteLine ("Updating all entities linked to '"  + entity.GetType().Name + "'.");
			
			foreach (var property in entity.GetType().GetProperties()) {
				if (Linker.IsLinkProperty (entity, property)) {
					UpdateLinkedEntities (entity, property);
				}
			}
		}

		public virtual void UpdateLinkedEntities(BaseEntity entity, PropertyInfo property)
		{
			var linkedEntities = Linker.GetLinkedEntities (entity, property);

			foreach (var e in linkedEntities) {
				if (Checker.Exists(e))
					Updater.Update (e);
			}
		}

		public virtual void CommitLinks(BaseEntity entity)
		{
			var previousEntity = Reader.Read(entity.GetType(), entity.Id);

			FindAndFixDifferences (previousEntity, entity);
		}

		public virtual void RemoveLinks (BaseEntity entity)
		{
			if (Settings.IsVerbose)
			{
				Console.WriteLine ("Removing links between '" + entity.GetType().Name + "' and other entities");
			}

			foreach (var property in entity.GetType().GetProperties()) {
				var otherPropertyName = "";

				// TODO: Clean up if statement
				if ((Linker.PropertyHasLinkAttribute(property, out otherPropertyName)
					|| Linker.IsLinkProperty(entity, property))
					&& !String.IsNullOrEmpty(otherPropertyName))
				{
					RemoveLinks (entity, property, otherPropertyName);
				}
			}
		}

		public virtual void RemoveLinks(BaseEntity entity, PropertyInfo property, string otherPropertyName)
		{
			if (!String.IsNullOrEmpty (otherPropertyName)) {
				var linkedEntities = Linker.GetLinkedEntities (entity, property);

				foreach (var linkedEntity in linkedEntities) {
					if (linkedEntity != null) {
						Linker.RemoveReturnLink (entity, property, linkedEntity, otherPropertyName);

						throw new NotImplementedException ();
						// Delay update until all references are fixed
						//Data.DelayUpdate (linkedEntity);
					}
				}
			}
		}

		public virtual void FindAndFixDifferences(BaseEntity previousEntity, BaseEntity updatedEntity)
		{
			foreach (var property in updatedEntity.GetType().GetProperties()) {
				if (Linker.IsLinkProperty (updatedEntity, property)) {
					FindAndFixDifferences (previousEntity, updatedEntity, property);
				}
			}
		}

		public virtual void FindAndFixDifferences(BaseEntity previousEntity, BaseEntity updatedEntity, PropertyInfo property)
		{
			var previousLinks = new BaseEntity[]{ };

			if (previousEntity != null)
				previousLinks = Linker.GetLinkedEntities (previousEntity, property);

			var updatedLinks = Linker.GetLinkedEntities (updatedEntity, property);

			var linksToAdd = IdentifyEntityLinksToAdd (previousLinks, updatedLinks);

			var linksToRemove = IdentifyEntityLinksToRemove (previousLinks, updatedLinks);

			CommitNewReverseLinks (updatedEntity, property, linksToAdd);

			RemoveOldReverseLinks (updatedEntity, property, linksToRemove);
		}

		public virtual BaseEntity[] IdentifyEntityLinksToAdd(BaseEntity[] previousLinkedEntities, BaseEntity[] updatedLinkedEntities)
		{
			var linksToAdd = (from entity in updatedLinkedEntities
				where !Linker.EntityExists (previousLinkedEntities, entity)
				select entity).ToArray ();

			return linksToAdd;
		}

		public virtual BaseEntity[] IdentifyEntityLinksToRemove(BaseEntity[] previousLinkedEntities, BaseEntity[] updatedLinkedEntities)
		{
			var linksToRemove = (from entity in previousLinkedEntities
				where !Linker.EntityExists (updatedLinkedEntities, entity)
				select entity).ToArray ();

			return linksToRemove;
		}

		public virtual void CommitNewReverseLinks(BaseEntity entity, PropertyInfo property, BaseEntity[] newLinkedEntities)
		{
			var otherPropertyName = Linker.GetOtherPropertyName (property);

			if (!String.IsNullOrEmpty (otherPropertyName)) {
				foreach (var newLinkedEntity in newLinkedEntities) {
					Linker.AddReturnLink (entity, property, newLinkedEntity, otherPropertyName);

					throw new NotImplementedException ();
					//if (!Data.PendingUpdate.Contains (newLinkedEntity))
					//	Data.DelayUpdate (newLinkedEntity);
				}
			}
		}

		public virtual void RemoveOldReverseLinks(BaseEntity entity, PropertyInfo property, BaseEntity[] oldLinkedEntities)
		{
			var otherPropertyName = Linker.GetOtherPropertyName (property);

			if (!String.IsNullOrEmpty (otherPropertyName)) {
				foreach (var oldLinkedEntity in oldLinkedEntities) {
					Linker.RemoveReturnLink (entity, property, oldLinkedEntity, otherPropertyName);

					throw new NotImplementedException ();
					//if (!Data.PendingUpdate.Contains (oldLinkedEntity))
					//	Data.DelayUpdate (oldLinkedEntity);
				}
			}
		}
	}
}

