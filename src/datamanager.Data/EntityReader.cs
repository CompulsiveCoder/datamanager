using System;
using Sider;
using datamanager.Entities;

namespace datamanager.Data
{
	public class EntityReader : BaseDataAdapter
	{
		public EntityReader ()
		{
		}

		public EntityReader (DataIdManager idManager) : base (idManager)
		{
		}

		public T Read<T>(string entityId)
		{
			var client = new RedisClient();
			var json = client.Get (new EntityKeys ().GetKey (typeof(T), entityId));

			if (String.IsNullOrEmpty (json))
				return default(T);

			var entity = new Parser().Parse<T> (json);

			return entity;
		}
	}
}

