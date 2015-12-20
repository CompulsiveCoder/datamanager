using System;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataKeys
	{
		public string Prefix;

		public DataKeys (string prefix)
		{
			Prefix = prefix;
		}

		public string GetKey(BaseEntity entity)
		{
			return GetKey (entity.GetType (), entity.Id);
		}

		public string GetKey(Type entityType, string id)
		{
			return Prefix + "-" + entityType.Name + "-" + id;
		}

		public string GetIdsKey(Type entityType)
		{
			return Prefix + "-" + entityType.Name + "-Ids";
		}
	}
}

