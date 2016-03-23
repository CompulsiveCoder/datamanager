using System;
using Sider;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataDeleter : BaseDataAdapter
	{
		public DataIdManager IdManager;
		public DataLinker Linker;
		public DataKeys Keys;

		public DataDeleter (DataIdManager idManager, DataKeys keys, DataLinker linker, BaseRedisClientWrapper client) : base (client)
		{
			IdManager = idManager;
			Keys = keys;
			Linker = linker;
		}

		public void Delete(BaseEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException ("entity");
			
			//Console.WriteLine ("Deleting: " + entity.GetType ().Name);

			Linker.RemoveLinks (entity);

			Client.Del(Keys.GetKey(entity));

			IdManager.Remove (entity);
		}
	}
}

