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

		public DataLinker ()
		{
			Data = new DataManager ();
		}

		public DataLinker (DataManager dataManager)
		{
			Data = dataManager;
		}

		public void CommitLinks(BaseEntity entity)
		{
			var previousEntity = Data.Get (entity.GetType(), entity.Id);

			FindAndFixDifferences (previousEntity, entity);
		
			// TODO: Finish off
			//throw new NotImplementedException ();
			/*if (entity.Log.HasEntries) {
				foreach (var logEntry in entity.Log.Entries) {
					if (logEntry is EntityLogAddEntry) {
						var target = (BaseEntity)logEntry.Link.Target;

						if (target.IsPendingLinkCommit) {
							target.IsPendingLinkCommit = false; // Mark as false here otherwise there will be an infinite loop
							Data.Save (target);
						}
					}
				}
			}*/

		}

		public void Link(BaseEntity left, BaseEntity right)
		{
			throw new NotImplementedException ();
			// Possibly obsolete
			/*foreach (var property in left.GetType().GetProperties()) {
				if (PropertyLinksToType (left, property, right.GetType ())) {

					var otherPropertyName = "";

					if (IsLinkedProperty (property, out otherPropertyName)) {
						AddReturnLink (left, property, right, otherPropertyName);
					}
				}
			}*/
		}

		public void RemoveLinks (BaseEntity entity)
		{
			//Console.WriteLine ("Removing links");

			var linker = new EntityLinker ();

			foreach (var property in entity.GetType().GetProperties()) {
				var otherPropertyName = "";

				// TODO: Clean up if statement
				if ((linker.PropertyHasLinkAttribute(property, out otherPropertyName)
					|| linker.IsLinkProperty(entity, property))
					&& !String.IsNullOrEmpty(otherPropertyName))
				{
					if (!String.IsNullOrEmpty (otherPropertyName)) {
						var linkedEntities = linker.GetLinkedEntities (entity, property);

						foreach (var linkedEntity in linkedEntities) {
							if (linkedEntity != null) {
								linker.RemoveReturnLink (entity, property, linkedEntity, otherPropertyName);

								// TODO: Delay update until all references are fixed
								Data.Update (linkedEntity);
							}
						}
					}
				}
			}
		}

		public void FindAndFixDifferences(BaseEntity previousEntity, BaseEntity updatedEntity)
		{
			var linker = new EntityLinker ();

			var allEntitiesPendingUpdate = new List<BaseEntity> ();

			foreach (var property in updatedEntity.GetType().GetProperties()) {
				if (linker.IsLinkProperty (updatedEntity, property)) {
					
					BaseEntity[] previousLinkedEntities;

					if (previousEntity != null)
						previousLinkedEntities = linker.GetLinkedEntities (previousEntity, property);
					else
						previousLinkedEntities = new BaseEntity[]{ };
					
					var updatedLinkedEntities = linker.GetLinkedEntities (updatedEntity, property);

					var newLinkedEntities = (from entity in updatedLinkedEntities
					                         where !linker.EntityExists (previousLinkedEntities, entity)
					                         select entity).ToArray ();

					allEntitiesPendingUpdate.AddRange (newLinkedEntities);

					var otherPropertyName = linker.GetOtherPropertyName (property);

					foreach (var newLinkedEntity in newLinkedEntities) {
						// TODO: Should the property object be passed in, instead of the property name only?
						linker.AddReturnLink (updatedEntity, property, newLinkedEntity, otherPropertyName);
					}
				}
			}

			//Console.WriteLine ("Updated links: " + allEntitiesPendingUpdate.Count);

			foreach (var entity in allEntitiesPendingUpdate) {
				Data.DelayUpdate (entity);
			}
		}
	}
}

