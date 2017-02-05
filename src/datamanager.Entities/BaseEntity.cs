using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace datamanager.Entities
{
	[Serializable]
	public class BaseEntity
	{
        public virtual string Id { get;set; }

        // TODO: Remove if not needed
		//public bool IsPendingLinkCommit = false;

        public string TypeName
        {
            get { return GetType ().Name; }
        }

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

