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
			var validatedEntity = entity.Clone ();

			// TODO: Clean up
//			throw new NotImplementedException ();
			// Remove the link log. It's serializable so that it can be transferred via serialization but it needs to be cleared before
			// it is stored in a database.
			// This isn't needed if the Log property is marked to be ignored during serialization but it's still worth leaving here to make sure.
			//validatedEntity.Log = null;

			return validatedEntity;
		}
	}
}

