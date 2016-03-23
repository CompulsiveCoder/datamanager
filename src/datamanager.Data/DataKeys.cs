using System;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataKeys
	{
		public DataManagerSettings Settings;

		public DataKeys (DataManagerSettings settings)
		{
			Settings = settings;
		}

		public string GetKey(BaseEntity entity)
		{
			return GetKey (entity.GetType ().Name, entity.Id);
		}

		public string GetKey(string entityType, string id)
		{
			return Settings.Prefix + "-" + entityType + "-" + id;
		}

		public string GetIdsKey(string entityType)
		{
			return Settings.Prefix + "-" + entityType + "-Ids";
		}

		public string GetTypesKey()
		{
			return Settings.Prefix + "-Types";
		}
	}
}

