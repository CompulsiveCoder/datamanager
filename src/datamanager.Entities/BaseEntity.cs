using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace datamanager.Entities
{
	[Serializable]
	public class BaseEntity
	{
		public string Id;

		public bool IsPendingLinkCommit = false;

		public BaseEntity ()
		{
			Id = Guid.NewGuid ().ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject (this);
		}

		public void AddLink(string propertyName, BaseEntity linkedEntity)
		{
			var adder = new EntityLinker ();

			adder.AddLink (this, propertyName, linkedEntity);
		}

		public void AddLinks(string propertyName, BaseEntity[] linkedEntities)
		{
			var adder = new EntityLinker ();

			foreach (var linkedEntity in linkedEntities) {
				adder.AddLink (this, propertyName, linkedEntity);
			}
		}

		public BaseEntity Clone()
		{
			return new EntityCloner ().Clone (this);
		}
	}
}

