using System;
using Sider;
using System.Collections.Generic;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataIdManager
	{
		public DataManager Data;

		public DataKeys Keys;

		public DataIdManager (DataManager dataManager)
		{
			Data = dataManager;
			Keys = new DataKeys (Data.Prefix);
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
			var idsKey = Keys.GetIdsKey (entityType);

			var idsString = Data.Client.Get (idsKey);

			var ids = new string[] { };

			if (!String.IsNullOrEmpty (idsString))
				ids = idsString.Split (',');

			return ids;
		}

		public void SetIds(Type entityType, string[] ids)
		{
			var idsKey = Keys.GetIdsKey (entityType);
			var idsString = String.Join (",", ids);
			Data.Client.Set(idsKey, idsString);
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

