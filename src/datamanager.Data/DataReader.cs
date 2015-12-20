using System;
using Sider;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataReader : BaseDataAdapter
	{
		public DataReader ()
		{
		}

		public DataReader (DataManager dataManager) : base (dataManager)
		{
		}

		public T Read<T>(string entityId)
		{
			var json = Data.Client.Get (Keys.GetKey (typeof(T), entityId));

			if (String.IsNullOrEmpty (json))
				return default(T);

			var entity = new Parser().Parse<T> (json);

			return entity;
		}
	}
}

