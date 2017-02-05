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
            return GetKey (entity.GetType().Name, entity.Id);
		}

		public string GetKey(string entityTypeName, string id)
		{
            var key = entityTypeName + "-" + id;

            key = AddPrefix (key);

            return key;
		}

        public string GetIdsKey(Type entityType)
        {
            return GetIdsKey (entityType.Name);
        }

		public string GetIdsKey(string entityTypeName)
        {
            var key = entityTypeName + ":Ids";

            key = AddPrefix (key);
            
            return key;
		}

		public string GetTypesKey()
		{
            var key = "Types";

            key = AddPrefix (key);

            return key;
		}

        public string AddPrefix(string key)
        {
            if (!String.IsNullOrEmpty (Settings.Prefix))
                key = Settings.Prefix + "-" + key;

            return key;
        }
	}
}

