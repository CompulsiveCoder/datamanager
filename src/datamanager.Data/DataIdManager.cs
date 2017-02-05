using System;
using System.Collections.Generic;
using datamanager.Entities;
using datamanager.Data.Providers;

namespace datamanager.Data
{
	public class DataIdManager
	{
        public DataManagerSettings Settings { get;set; }
		public DataKeys Keys { get; set; }

        public BaseDataProvider Provider { get; set; }

        public DataIdManager (DataManagerSettings settings, DataKeys keys, BaseDataProvider provider)
        {
            if (settings == null)
                throw new ArgumentNullException ("settings");
            
			if (keys == null)
				throw new ArgumentNullException ("keys");

			if (provider == null)
				throw new ArgumentNullException ("client");
			
            Settings = settings;

			Keys = keys;
			Provider = provider;
		}

		public void Add(BaseEntity entity)
		{
            Add (entity.GetType(), entity.Id);
		}

        public void Add(Type entityType, string id)
        {
            var ids = new List<string>(GetIds (entityType.Name));

            if (!ids.Contains (id))
                ids.Add (id);

            SetIds (entityType, ids.ToArray ());

            AddIdForIndexTypes (entityType, id);
        }

        protected void AddIdForIndexTypes(Type entityType, string id)
        {
            foreach (var a in entityType.GetCustomAttributes(true)) {
                if (a is IndexTypeAttribute) {
                    var attribute = (IndexTypeAttribute)a;

                    Add (attribute.IndexType, id);
                }
            }
        }


		public void Remove(BaseEntity entity)
		{
			var ids = new List<string>(GetIds (entity.GetType().Name));

			if (!ids.Contains (entity.Id))
				ids.Remove (entity.Id);

			SetIds (entity.GetType (), ids.ToArray ());
		}

		public string[] GetIds(string entityType)
		{
            if (Settings.IsVerbose) {
                Console.WriteLine ("    Getting IDs");
                Console.WriteLine ("      Entity type: " + entityType);
            }

			var idsKey = Keys.GetIdsKey (entityType);

            if (Settings.IsVerbose)
                Console.WriteLine ("      IDs key: " + idsKey);

            var idsString = Provider.Get (idsKey);

            if (Settings.IsVerbose)
                Console.WriteLine ("      IDs string: " + idsString);

			var ids = new string[] { };

			if (!String.IsNullOrEmpty (idsString))
				ids = idsString.Split (',');

            if (Settings.IsVerbose)
                Console.WriteLine ("      IDs total: " + ids.Length);
            
			return ids;
		}

		public void SetIds(Type entityType, string[] ids)
		{
			var idsKey = Keys.GetIdsKey (entityType.Name);
			var idsString = String.Join (",", ids);
			Provider.Set(idsKey, idsString);
		}

		public string[] ConvertToStringArray(Guid[] ids)
		{
			var idStrings = new List<string> ();

			foreach (Guid id in ids)
				idStrings.Add (id.ToString());

			return idStrings.ToArray ();
		}
	}
}

