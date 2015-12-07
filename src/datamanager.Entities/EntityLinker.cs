using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Collections;

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
				var value = property.GetValue (entity);
				var newValue = AddEntityToObject (linkedEntity, value, property.PropertyType);
				property.SetValue (entity, newValue);

				entity.IsPendingLinkCommit = true;

				entity.Log.Add (new EntityLogAddEntry (entity.GetLink()));

				if (!String.IsNullOrEmpty(otherPropertyName))
					AddReturnLink (entity, property, linkedEntity, otherPropertyName);
			} else
				throw new Exception ("Invalid property. It needs to have the type EntityLink.");
		}

		public void RemoveReturnLink(BaseEntity entity, PropertyInfo property, BaseEntity targetEntity, string targetEntityPropertyName)
		{
			var targetEntityProperty = targetEntity.GetType ().GetProperty (targetEntityPropertyName);

			var existingReturnLinksObject = targetEntityProperty.GetValue (targetEntity);

			var newReturnLinksObject = RemoveEntityFromObject (entity, existingReturnLinksObject, targetEntityProperty);

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
			return IsEntityLinkProperty (property)
			|| IsEntityArrayLinkProperty (property);
		}

		public bool IsEntityLinkProperty(PropertyInfo property)
		{
			var isEntity = property.PropertyType.IsSubclassOf (typeof(BaseEntity));

			return isEntity;
		}

		public bool IsEntityArrayLinkProperty(PropertyInfo property)
		{
			var isEntityArray = property.PropertyType.IsArray
				&& property.PropertyType.GetElementType ().IsSubclassOf (typeof(BaseEntity));

			return isEntityArray;
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

			var newReturnLinksObject = AddEntityToObject (entity, existingReturnLinksObject, targetEntityProperty.PropertyType);

			entity.Log.Add (new EntityLogAddEntry (entity.GetLink()));

			targetEntityProperty.SetValue (targetEntity, newReturnLinksObject);
		}

		public object AddEntityToObject(BaseEntity entityToAdd, object linksObject, Type propertyType)
		{
			if (propertyType.IsSubclassOf(typeof(BaseEntity))) {
				return entityToAdd;
			}
			else if (propertyType.IsArray
				&& propertyType.GetElementType().IsSubclassOf(typeof(BaseEntity))) {

				var list = new ArrayList ();

				if (linksObject != null)
					list.AddRange ((BaseEntity[])linksObject);
				
				// TODO: Should this check that it's not already in the list? Check via id, not via object comparison
				if (!EntityExists((BaseEntity[])list.ToArray(typeof(BaseEntity)), entityToAdd))
					list.Add (entityToAdd);

				return list.ToArray(entityToAdd.GetType());
			} else
				throw new Exception ("Invalid return link object. Must be EntityLink or EntityLink[].");

		}

		public object RemoveEntityFromObject(BaseEntity entityToRemove, object linksObject, PropertyInfo property)
		{
			// If the property type is a single entity, return the new entity
			if (IsEntityLinkProperty(property)) {
				return null;
			}
			else if (IsEntityArrayLinkProperty(property)) {
				var list = new ArrayList ();

				if (linksObject != null)
					list.AddRange ((BaseEntity[])linksObject);

				for (int i = 0; i < list.Count; i++) {
					if (((BaseEntity)list [i]).Id == entityToRemove.Id) {
						list.RemoveAt (i);
						break;
					}
				}

				return list.ToArray (entityToRemove.GetType());
			} else
				throw new Exception ("Invalid return link object. Must be subclass of BaseEntity or BaseEntity[] array.");

		}

		public EntityLink[] GetLinks (BaseEntity entity, PropertyInfo property)
		{
			throw new NotImplementedException ();
			/*var value = property.GetValue (entity);

			if (IsEntityLinkProperty(property)) {
				return new BaseEntity[]{value};
			} else if (IsEntityArrayLinkProperty(property)) {
				return (EntityLink[])value;
			}

			return new EntityLink[]{ };*/
		}

		public BaseEntity[] GetLinkedEntities (BaseEntity entity, PropertyInfo property)
		{
			var list = new List<BaseEntity> ();

			var value = property.GetValue (entity);

			if (IsEntityLinkProperty (property)) {
				list.Add ((BaseEntity)value);
			} else {
				list.AddRange ((BaseEntity[])value);
			}

			return list.ToArray ();
		}

		public bool EntityExists(BaseEntity[] entities, BaseEntity entityToFind)
		{
			return (from entity in entities
				where entity.GetType().FullName == entityToFind.GetType().FullName
			            && entity.Id == entityToFind.Id
				select entity).Count() > 0;
		}
	}
}

