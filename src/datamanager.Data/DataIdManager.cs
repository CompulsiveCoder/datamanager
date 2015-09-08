using System;
using Sider;
using System.Collections.Generic;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataIdManager
	{
		public DataIdManager ()
		{
		}

		public void Add(BaseEntity entity)
		{
			var ids = new List<string>(GetIds (entity.GetType()));

			if (!ids.Contains (entity.Id))
				ids.Add (entity.Id);

			SetIds (entity.GetType (), ids.ToArray ());
		}


		public void Remove(BaseEntity entity)
		{
			var ids = new List<string>(GetIds (entity.GetType()));

			if (!ids.Contains (entity.Id))
				ids.Remove (entity.Id);

			SetIds (entity.GetType (), ids.ToArray ());
		}

		public string[] GetIds(Type entityType)
		{
			var idsKey = new EntityKeys ().GetIdsKey (entityType);

			var client = new RedisClient();
			var idsString = client.Get (idsKey);

			var ids = new string[] { };

			if (!String.IsNullOrEmpty (idsString)) {
				var idsArray = idsString.Split (',');


				ids = idsArray;
				// TODO: Clean up
				//ids = ConvertToGuidArray (idsArray);

			}
			return ids;
		}

		public void SetIds(Type entityType, string[] ids)
		{
			var idsKey = new EntityKeys ().GetIdsKey (entityType);
			var client = new RedisClient();
			var idsString = String.Join (",", ids);//ConvertToStringArray(ids)); // TODO: Remove if not needed
			client.Set(idsKey, idsString);
		}

		// TODO: Remove if not needed
		public Guid[] ConvertToGuidArray(string[] idsArray)
		{
			var ids = new List<Guid> ();

			foreach (string idString in idsArray)
				ids.Add (Guid.Parse (idString));

			return ids.ToArray ();
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

