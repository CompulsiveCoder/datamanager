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

						// TODO: Delay update until all references are fixed
						Data.Update (linkedEntity);
					}
				}
			}
		}

		public void FindAndFixDifferences(BaseEntity previousEntity, BaseEntity updatedEntity)
		{
			// TODO: Clean up
			//var allEntitiesPendingUpdate = new List<BaseEntity> ();

			foreach (var property in updatedEntity.GetType().GetProperties()) {
				if (Linker.IsLinkProperty (updatedEntity, property)) {
					FindAndFixDifferences (previousEntity, updatedEntity, property);
				}
			}

			//Console.WriteLine ("Updated links: " + allEntitiesPendingUpdate.Count);

			//foreach (var entity in allEntitiesPendingUpdate) {
			//	Data.DelayUpdate (entity);
			//}
		}

		public void FindAndFixDifferences(BaseEntity previousEntity, BaseEntity updatedEntity, PropertyInfo property)
		{
		//	FindAndFixNewLinks(previousE

			//var newLinkedEntities = GetNewLinkedEntities ();
			//var oldLinkedEntities = GetLinkedEntitiesMissingFromEntity();
		
			var previousLinkedEntities = GetLinkedEntitiesFromPreviousEntity (previousEntity, property);

			SyncNewReverseLinks (updatedEntity, property, newLinkedEntities);

			RemoveOldReverseLinks (updatedEntity, property, oldLinkedEntities);
		}

		public BaseEntity[] GetNewLinkedEntities()
		{
			var previousLinkedEntities = GetLinkedEntitiesFromPreviousEntity (previousEntity, property);
			var updatedLinkedEntities = GetLinkedEntitiesFromUpdatedEntity (previousEntity, property);

			var newLinkedEntities = (from entity in updatedLinkedEntities
				where !Linker.EntityExists (previousLinkedEntities, entity)
				select entity).ToArray ();
		}

		public BaseEntity[] GetLinkedEntitiesFromPreviousEntity(BaseEntity previousEntity, PropertyInfo property)
		{
			if (previousEntity != null)
				return Linker.GetLinkedEntities (previousEntity, property);
			else
				return new BaseEntity[]{ };
		}

		public BaseEntity[] GetLinkedEntitiesFromUpdatedEntity(BaseEntity updatedEntity, PropertyInfo property)
		{
			var updatedLinkedEntities = Linker.GetLinkedEntities (updatedEntity, property);

			return updatedLinkedEntities;
		}

		public BaseEntity[] GetLinkedEntitiesMissingFromEntity(BaseEntity[] previousLinkedEntities, PropertyInfo updatedLinkedEntities)
		{
			var oldLinkedEntities = (from entity in previousLinkedEntities
			where !Linker.EntityExists (updatedLinkedEntities, entity)
			select entity).ToArray ();
		}

		public void SyncNewReverseLinks(BaseEntity entity, PropertyInfo property, BaseEntity[] newLinkedEntities)
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

