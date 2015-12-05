using System;
using Sider;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataUpdater : BaseDataAdapter
	{
		public DataUpdater ()
		{
		}

		public DataUpdater (DataManager dataManager) : base (dataManager)
		{
		}

		public void Update(BaseEntity entity)
		{
			Console.WriteLine ("Updating: " + entity.GetType ().Name);

			var key = new DataKeys ().GetKey (entity);
			Data.Client.Set (key, Data.Preparer.PrepareForStorage(entity).ToJson ());
		}
	}
}

