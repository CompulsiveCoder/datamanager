using System;
using Sider;
using System.Collections.Generic;
using datamanager.Entities;

namespace datamanager.Data
{
	public class EntityLister : BaseDataAdapter
	{
		public EntityLister ()
		{
		}

		public T[] Get<T>()
		{
			var ids = IdManager.GetIds(typeof(T));

			var entities = new List<T> ();
			var reader = new EntityReader ();
			foreach (string id in ids) {
				entities.Add (reader.Read<T>(id));
			}
			return entities.ToArray();
		}
	}
}

