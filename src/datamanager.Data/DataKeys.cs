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
			return GetKey (entity.Id);
		}

		public string GetKey(string id)
		{
			return Settings.Prefix + "-" + id;
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

