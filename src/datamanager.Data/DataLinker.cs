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
		public DataManager Data { get; set; }

		public EntityLinker Linker { get; set; }

		public DataLinker ()
		{
			Data = new DataManager ();
			Linker = new EntityLinker ();
		}

		public DataLinker (DataManager dataManager)
		{
			Data = dataManager;
			Linker = new EntityLinker ();
		}

		public void SaveLinkedEntities(BaseEntity entity)
		{
			if (Data.IsVerbose)
				Console.WriteLine ("Saving all entities linked to '"  + entity.GetType().Name + "'.");
			
			foreach (var property in entity.GetType().GetProperties()) {
				if (Linker.IsLinkProperty (entity, property)) {
					SaveLinkedEntities (entity, property);
				}
			}
		}

		public void SaveLinkedEntities(BaseEntity entity, PropertyInfo property)
		{
			var linkedEntities = Linker.GetLinkedEntities (entity, property);

			foreach (var e in linkedEntities) {
				if (!Data.Exists(e))
					Data.Save (e);
			}
		}


		public void UpdateLinkedEntities(BaseEntity entity)
		{
			if (Data.IsVerbose)
				Console.WriteLine ("Updating all entities linked to '"  + entity.GetType().Name + "'.");
			
			foreach (var property in entity.GetType().GetProperties()) {
				if (Linker.IsLinkProperty (entity, property)) {
					UpdateLinkedEntities (entity, property);
				}
			}
		}

		public void UpdateLinkedEntities(BaseEntity entity, PropertyInfo property)
		{
			var linkedEntities = Linker.GetLinkedEntities (entity, property);

			foreach (var e in linkedEntities) {
				if (Data.Exists(e))
					Data.Update (e);
			}
		}

		public void CommitLinks(BaseEntity entity)
		{
			var previousEntity = Data.Get (entity.GetType(), entity.Id);

			FindAndFixDifferences (previousEntity, entity);
		}

		public void RemoveLinks (BaseEntity entity)
		{
			if (Data.IsVerbose)
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

		public void RemoveLinks(BaseEntity entity, PropertyInfo property, string otherPropertyName)
		{
			if (!String.IsNullOrEmpty (otherPropertyName)) {
				var linkedEntities = Linker.GetLinkedEntities (entity, property);

				foreach (var linkedEntity in linkedEntities) {
					if (linkedEntity != null) {
						Linker.RemoveReturnLink (entity, property, linkedEntity, otherPropertyName);

						// Delay update until all references are fixed
						Data.DelayUpdate (linkedEntity);
					}
				}
			}
		}

		public void FindAndFixDifferences(BaseEntity previousEntity, BaseEntity updatedEntity)
		{
			foreach (var property in updatedEntity.GetType().GetProperties()) {
				if (Linker.IsLinkProperty (updatedEntity, property)) {
					FindAndFixDifferences (previousEntity, updatedEntity, property);
				}
			}
		}

		public void FindAndFixDifferences(BaseEntity previousEntity, BaseEntity updatedEntity, PropertyInfo property)
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

		public BaseEntity[] IdentifyEntityLinksToAdd(BaseEntity[] previousLinkedEntities, BaseEntity[] updatedLinkedEntities)
		{
			var linksToAdd = (from entity in updatedLinkedEntities
				where !Linker.EntityExists (previousLinkedEntities, entity)
				select entity).ToArray ();

			return linksToAdd;
		}

		public BaseEntity[] IdentifyEntityLinksToRemove(BaseEntity[] previousLinkedEntities, BaseEntity[] updatedLinkedEntities)
		{
			var linksToRemove = (from entity in previousLinkedEntities
				where !Linker.EntityExists (updatedLinkedEntities, entity)
				select entity).ToArray ();

			return linksToRemove;
		}

		public void CommitNewReverseLinks(BaseEntity entity, PropertyInfo property, BaseEntity[] newLinkedEntities)
		{
			var otherPropertyName = Linker.GetOtherPropertyName (property);

			if (!String.IsNullOrEmpty (otherPropertyName)) {
				foreach (var newLinkedEntity in newLinkedEntities) {
					Linker.AddReturnLink (entity, property, newLinkedEntity, otherPropertyName);

					if (!Data.PendingUpdate.Contains (newLinkedEntity))
						Data.DelayUpdate (newLinkedEntity);
				}
			}
		}

		public void RemoveOldReverseLinks(BaseEntity entity, PropertyInfo property, BaseEntity[] oldLinkedEntities)
		{
			var otherPropertyName = Linker.GetOtherPropertyName (property);

			if (!String.IsNullOrEmpty (otherPropertyName)) {
				foreach (var oldLinkedEntity in oldLinkedEntities) {
					Linker.RemoveReturnLink (entity, property, oldLinkedEntity, otherPropertyName);

					if (!Data.PendingUpdate.Contains (oldLinkedEntity))
						Data.DelayUpdate (oldLinkedEntity);
				}
			}
		}
	}
}

