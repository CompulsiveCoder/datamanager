using System;
using Sider;
using System.Collections.Generic;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataLister : BaseDataAdapter
	{
		public DataLister ()
		{
		}

		public DataLister (DataManager dataManager) : base (dataManager)
		{
		}

		public T[] Get<T>()
		{
			var ids = Data.IdManager.GetIds(typeof(T));

			var entities = new List<T> ();
			foreach (string id in ids) {
				entities.Add (Data.Reader.Read<T>(id));
			}
			return entities.ToArray();
		}
	}
}

