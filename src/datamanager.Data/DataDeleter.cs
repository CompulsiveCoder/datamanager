using System;
using datamanager.Entities;
using datamanager.Data.Providers;

namespace datamanager.Data
{
	public class DataDeleter : BaseDataAdapter
	{
		public DataIdManager IdManager;
		public DataLinker Linker;
		public DataKeys Keys;

        public DataDeleter (DataIdManager idManager, DataKeys keys, DataLinker linker, BaseDataProvider provider) : base (provider)
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

			Provider.Delete(Keys.GetKey(entity));

			IdManager.Remove (entity);
		}
	}
}

