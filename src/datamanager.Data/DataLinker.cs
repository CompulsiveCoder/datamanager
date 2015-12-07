using System;
using datamanager.Entities;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

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
			Console.WriteLine ("Removing links");

			var linker = new EntityLinker ();

			foreach (var property in entity.GetType().GetProperties()) {
				var otherPropertyName = "";

				// TODO: Clean up if statement
				if ((linker.PropertyHasLinkAttribute(property, out otherPropertyName)
					|| linker.IsLinkProperty(property))
					&& !String.IsNullOrEmpty(otherPropertyName))
				{
					if (!String.IsNullOrEmpty (otherPropertyName)) {
						var linkedEntities = linker.GetLinkedEntities (entity, property);

						foreach (var linkedEntity in linkedEntities) {
							linker.RemoveReturnLink (entity, property, linkedEntity, otherPropertyName);

							// TODO: Delay update until all references are fixed
							Data.Update (linkedEntity);
						}
					}
				}
			}
		}
	}
}

