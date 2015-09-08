using System;
using Sider;
using datamanager.Entities;

namespace datamanager.Data
{
	public class EntityDeleter : BaseDataAdapter
	{
		public EntityDeleter ()
		{
		}

		public EntityDeleter (DataIdManager idManager) : base (idManager)
		{
		}

		public void Delete(BaseEntity entity)
		{
			var client = new RedisClient();
			client.Del(new EntityKeys().GetKey(entity));

			IdManager.Remove (entity);
		}
	}
}

