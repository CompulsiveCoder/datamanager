using System;
using Newtonsoft.Json;

namespace datamanager.Entities
{
	[Serializable]
	public class BaseEntity
	{
		public string Id;

		[JsonIgnore]
		[NonSerialized]
		public EntityLog Log;

		public bool IsPendingLinkCommit = false;

		public BaseEntity ()
		{
			Id = Guid.NewGuid ().ToString();
			Log = new EntityLog ();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject (this);
		}

		public EntityLink GetLink()
		{
			return new EntityLink(this);
		}

		public void AddLink(string propertyName, BaseEntity linkedEntity)
		{
			var adder = new EntityLinker ();

			adder.AddLink (this, propertyName, linkedEntity);
		}

		public BaseEntity Clone()
		{
			return new EntityCloner ().Clone (this);
		}
	}
}

