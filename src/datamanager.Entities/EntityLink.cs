using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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

		public T To<T>()
			where T : BaseEntity
		{
			return (T)Target;
		}

		public override string ToString ()
		{
			return string.Format ("[EntityLink: TypeName={0}, Id={1}]", TypeName, Id);
		}

		static public EntityLink GetLink(BaseEntity entity)
		{
			return entity.GetLink ();
		}

		static public EntityLink[] GetLinks(BaseEntity[] entities)
		{
			return (from entity in entities
			        select entity.GetLink ()).ToArray ();
		}

		static public T[] GetEntities<T>(EntityLink[] links)
			where T : BaseEntity
		{
			return (from link in links
				select (T)link.Target).ToArray ();
		}
	}
}

