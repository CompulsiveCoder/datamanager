using System;
using datamanager.Entities;
using datamanager.Data.Providers;

namespace datamanager.Data
{
	public class DataReader : BaseDataAdapter
	{
		public DataTypeManager TypeManager;
		public DataIdManager IdManager;
		public DataKeys Keys;

        public DataReader (DataTypeManager typeManager, DataIdManager idManager, DataKeys keys, BaseDataProvider provider) : base (provider)
		{
			TypeManager = typeManager;
			IdManager = idManager;
			Keys = keys;
		}

		public T Read<T>(string entityId)
		{
            var json = Provider.Get (Keys.GetKey (typeof(T).Name, entityId));

			if (String.IsNullOrEmpty (json))
				return default(T);

			var entity = new Parser().Parse<T> (json);

			return entity;
		}

		public BaseEntity Read(string entityTypeName, string entityId)
		{
			if (TypeManager.Exists (entityTypeName)) {
				var entityType = TypeManager.GetType (entityTypeName);
				return Read (entityType, entityId);
			} else
				return null;
		}

		public BaseEntity Read(Type entityType, string entityId)
		{
			if (entityType == null)
				throw new ArgumentNullException ("entityType");
			
            var key = Keys.GetKey (entityType.Name, entityId);

			var json = Provider.Get (key);

			if (String.IsNullOrEmpty (json))
				return null;
			
			var entity = new Parser ().Parse (entityType, json);

			return entity;
		}
	}
}

