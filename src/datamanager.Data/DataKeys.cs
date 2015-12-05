using System;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataKeys
	{
		public DataKeys ()
		{
		}

		public string GetKey(BaseEntity entity)
		{
			return GetKey (entity.GetType (), entity.Id);
		}

		public string GetKey(Type entityType, string id)
		{
			return DataConfig.Prefix + "-" + entityType.Name + "-" + id;
		}

		public string GetIdsKey(Type entityType)
		{
			return DataConfig.Prefix + "-" + entityType.Name + "-Ids";
		}
	}
}

