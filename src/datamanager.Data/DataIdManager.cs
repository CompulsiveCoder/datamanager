using System;
using Sider;
using System.Collections.Generic;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataIdManager
	{
		public DataKeys Keys { get; set; }

		public BaseRedisClientWrapper Client { get; set; }

		public DataIdManager (DataKeys keys, BaseRedisClientWrapper client)
		{
			if (keys == null)
				throw new ArgumentNullException ("keys");

			if (client == null)
				throw new ArgumentNullException ("client");
			
			Keys = keys;
			Client = client;
		}

		public void Add(BaseEntity entity)
		{
			var ids = new List<string>(GetIds (entity.GetType().Name));

			if (!ids.Contains (entity.Id))
				ids.Add (entity.Id);

			SetIds (entity.GetType (), ids.ToArray ());
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
			var idsKey = Keys.GetIdsKey (entityType);

			var idsString = Client.Get (idsKey);

			var ids = new string[] { };

			if (!String.IsNullOrEmpty (idsString))
				ids = idsString.Split (',');

			return ids;
		}

		public void SetIds(Type entityType, string[] ids)
		{
			var idsKey = Keys.GetIdsKey (entityType.Name);
			var idsString = String.Join (",", ids);
			Client.Set(idsKey, idsString);
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

