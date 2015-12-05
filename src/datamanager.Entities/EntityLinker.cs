using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace datamanager.Entities
{
	public class EntityLinker
	{
		public EntityLinker ()
		{
		}

		public void AddLink(BaseEntity entity, string propertyName, BaseEntity linkedEntity)
		{
			var otherPropertyName = "";

			var property = entity.GetType ().GetProperty (propertyName);

			if (PropertyHasLinkAttribute (property, out otherPropertyName)
				|| IsLinkProperty(property)) {
				EntityLink link = null;

				var value = property.GetValue (entity);
				var newValue = AddLinkToObject (linkedEntity, value, property.PropertyType, out link);
				property.SetValue (entity, newValue);

				entity.IsPendingLinkCommit = true;

				entity.Log.Add (new EntityLogAddEntry (link));

				if (!String.IsNullOrEmpty(otherPropertyName))
					AddReturnLink (entity, property, linkedEntity, otherPropertyName);
			} else
				throw new Exception ("Invalid property. It needs to have the type EntityLink.");
		}

		public void RemoveReturnLink(BaseEntity entity, PropertyInfo property, BaseEntity targetEntity, string targetEntityPropertyName)
		{
			var targetEntityProperty = targetEntity.GetType ().GetProperty (targetEntityPropertyName);

			var existingReturnLinksObject = targetEntityProperty.GetValue (targetEntity);

			var newReturnLinksObject = RemoveLinkFromObject (entity, existingReturnLinksObject, targetEntityProperty.PropertyType);

			targetEntityProperty.SetValue (targetEntity, newReturnLinksObject);
		}

		public bool PropertyHasLinkAttribute(PropertyInfo property, out string otherPropertyName)
		{
			var attributes = property.GetCustomAttributes ();

			foreach (var attribute in attributes) {
				if (attribute is OneWayAttribute) {
					otherPropertyName = "";
					return true;
				}
				else if (attribute is TwoWayAttribute) {
					otherPropertyName = ((TwoWayAttribute)attribute).OtherPropertyName;
					return true;
				}
			}

			otherPropertyName = "";

			return false;
		}

		public bool IsLinkProperty(PropertyInfo property)
		{
			return property.PropertyType.FullName == typeof(EntityLink).FullName
					|| property.PropertyType.FullName == typeof(EntityLink[]).FullName;
		}

		public bool PropertyLinksToType(BaseEntity entity, PropertyInfo property, Type targetEntityType)
		{
			var summary = (EntityLink)property.GetValue (entity);

			return targetEntityType.FullName == summary.TypeName;
		}

		public void AddReturnLink(BaseEntity entity, PropertyInfo property, BaseEntity targetEntity, string targetEntityPropertyName)
		{
			var targetEntityProperty = targetEntity.GetType ().GetProperty (targetEntityPropertyName);

			var existingReturnLinksObject = targetEntityProperty.GetValue (targetEntity);

			EntityLink link = null;

			var newReturnLinksObject = AddLinkToObject (entity, existingReturnLinksObject, targetEntityProperty.PropertyType, out link);

			entity.Log.Add (new EntityLogAddEntry (link));

			targetEntityProperty.SetValue (targetEntity, newReturnLinksObject);
		}

		public object AddLinkToObject(BaseEntity entityToAdd, object linksObject, Type propertyType, out EntityLink link)
		{
			// If the property type is a single entity, return the new entity
			if (propertyType.FullName == typeof(EntityLink).FullName) {
				link = entityToAdd.GetLink ();
				return link;
			}
			else if (propertyType.IsArray
				&& propertyType.FullName == typeof(EntityLink[]).FullName) {

				var list = new List<EntityLink> ();

				if (linksObject != null)
					list.AddRange ((EntityLink[])linksObject);

				link = entityToAdd.GetLink ();

				// TODO: Should this check that it's not already in the list? Check via id, not via object comparison
				if (!LinkExists(list.ToArray(), link))
					list.Add (link);

				return list.ToArray ();
			} else
				throw new Exception ("Invalid return link object. Must be EntityLink or EntityLink[].");

		}

		public object RemoveLinkFromObject(BaseEntity entityToRemove, object linksObject, Type propertyType)
		{
			// If the property type is a single entity, return the new entity
			if (propertyType.FullName == typeof(EntityLink).FullName) {
				return null;
			}
			else if (propertyType.IsArray
				&& propertyType.FullName == typeof(EntityLink[]).FullName) {

				var list = new List<EntityLink> ();

				if (linksObject != null)
					list.AddRange ((EntityLink[])linksObject);

				for (int i = 0; i < list.Count; i++) {
					if (list [i].Id == entityToRemove.Id) {
						list.RemoveAt (i);
						break;
					}
				}

				return list.ToArray ();
			} else
				throw new Exception ("Invalid return link object. Must be EntityLink or EntityLink[].");

		}

		public EntityLink[] GetLinks (BaseEntity entity, PropertyInfo property)
		{
			var value = property.GetValue (entity);

			if (property.PropertyType.FullName == typeof(EntityLink).FullName) {
				return new EntityLink[]{(EntityLink)value};
			} else if (property.PropertyType.IsArray
				&& property.PropertyType.FullName == typeof(EntityLink[]).FullName) {
				return (EntityLink[])value;
			}

			return new EntityLink[]{ };
		}

		public BaseEntity[] GetLinkedEntities (BaseEntity entity, PropertyInfo property)
		{
			var list = new List<BaseEntity> ();

			foreach (var link in GetLinks(entity, property)) {
				list.Add (link.Target);
			}

			return list.ToArray ();
		}

		public bool LinkExists(EntityLink[] links, EntityLink linkToFind)
		{
			return (from link in links
			        where link.TypeName == linkToFind.TypeName
			            && link.Id == linkToFind.Id
				select link).Count() > 0;
		}
	}
}

