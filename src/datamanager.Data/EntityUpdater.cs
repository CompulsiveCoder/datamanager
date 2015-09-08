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

		public void Update(BaseEntity entity)
		{
			var client = new RedisClient ();
			if (client.Exists (entity))
				throw new AlreadyExistsException ();
			else
				client.Set (new EntityKeys ().GetKey (entity), entity.ToJson ());
		}
	}
}

