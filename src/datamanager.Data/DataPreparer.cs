using System;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataPreparer : BaseDataAdapter
	{
		public DataPreparer (BaseRedisClientWrapper client) : base(client)
		{
		}

		public BaseEntity PrepareForStorage(BaseEntity entity)
		{
            // TODO: Implement clean up of entity if needed

			return entity;
		}
	}
}

