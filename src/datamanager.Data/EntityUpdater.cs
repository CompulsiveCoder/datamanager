using System;
using datamanager.Entities;
using Sider;

namespace datamanager.Data
{
	public class EntityUpdater : BaseDataAdapter
	{
		public EntityUpdater ()
		{
		}

		public EntityUpdater (DataIdManager idManager) : base (idManager)
		{
		}

		public void Update(BaseEntity entity)
		{
			var key = new EntityKeys ().GetKey (entity);
			var client = new RedisClient ();
			if (client.Exists (key))
				throw new AlreadyExistsException ();
			else
				client.Set (key, entity.ToJson ());
		}
	}
}

