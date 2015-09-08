using System;
using Sider;
using datamanager.Entities;

namespace datamanager.Data
{
	public class EntityReader
	{
		public EntityReader ()
		{
		}

		public T Read<T>(string entityId)
		{
			var client = new RedisClient();
			var json = client.Get (new EntityKeys ().GetKey (typeof(T), entityId));

			var entity = new Parser().Parse<T> (json);

			return entity;
		}
	}
}

