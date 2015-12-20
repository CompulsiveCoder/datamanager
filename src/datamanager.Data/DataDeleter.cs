using System;
using Sider;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataDeleter : BaseDataAdapter
	{
		public DataDeleter ()
		{
		}

		public DataDeleter (DataManager dataManager) : base (dataManager)
		{
		}

		public void Delete(BaseEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException ("entity");
			
			//Console.WriteLine ("Deleting: " + entity.GetType ().Name);

			Data.Linker.RemoveLinks (entity);

			Data.Client.Del(Keys.GetKey(entity));

			Data.IdManager.Remove (entity);
		}
	}
}

