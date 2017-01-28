using System;
using System.Collections.Generic;
using datamanager.Entities;
using datamanager.Data.Providers;

namespace datamanager.Data
{
	public class DataLister : BaseDataAdapter
	{
        public DataManagerSettings Settings;
        public DataTypeManager TypeManager;
		public DataIdManager IdManager;
		public DataReader Reader;

        public DataLister (DataManagerSettings settings, DataTypeManager typeManager, DataIdManager idManager, DataReader reader, BaseDataProvider provider) : base (provider)
		{
            Settings = settings;
			TypeManager = typeManager;
			IdManager = idManager;
			Reader = reader;
		}


		public T[] Get<T>()
		{
            if (Settings.IsVerbose) {
                Console.WriteLine ("Getting entities");
                Console.WriteLine ("Type: " + typeof(T).AssemblyQualifiedName);
            }

			var ids = IdManager.GetIds(typeof(T).Name);

			var entities = new List<T> ();
			foreach (string id in ids) {
				entities.Add (Reader.Read<T>(id));
			}
			return entities.ToArray();
		}

		public BaseEntity[] Get(string entityTypeName)
		{
			var ids = IdManager.GetIds(entityTypeName);

			var entities = new List<BaseEntity> ();
			foreach (string id in ids) {
				entities.Add (Reader.Read(entityTypeName, id));
			}
			return entities.ToArray();
		}

		public BaseEntity[] GetAll()
		{
			var entities = new List<BaseEntity> ();

			var types = TypeManager.GetTypeNames ();

			foreach (var typeName in types) {
				var ids = IdManager.GetIds (typeName);

				foreach (string id in ids) {
					entities.Add (Reader.Read (typeName, id));
				}
			}
			return entities.ToArray();
		}
	}
}

