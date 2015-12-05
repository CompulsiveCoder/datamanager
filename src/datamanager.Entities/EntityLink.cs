using System;
using Newtonsoft.Json;

namespace datamanager.Entities
{
	[Serializable]
	public class EntityLink
	{
		public string TypeName { get;set; }

		public string Id { get; set; }

		public String[][] Properties { get; set; }

		// TODO: Add lazy loading
		[NonSerialized]
		[JsonIgnore]
		public BaseEntity Target;

		public EntityLink()
		{
		}

		public EntityLink(BaseEntity entity)
		{
			Id = entity.Id;
			TypeName = entity.GetType ().FullName;
			Target = entity;
		}

		public override string ToString ()
		{
			return string.Format ("[EntityLink: TypeName={0}, Id={1}]", TypeName, Id);
		}
	}
}

