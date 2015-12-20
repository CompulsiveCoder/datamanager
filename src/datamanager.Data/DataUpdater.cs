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
			if (Data.Exists (entity)) {
				if (Data.IsVerbose)
					Console.WriteLine ("Updating: " + entity.GetType ().Name);

				Data.Linker.CommitLinks (entity);

				var key = Keys.GetKey (entity);
				Data.Client.Set (key, Data.Preparer.PrepareForStorage (entity).ToJson ());
			} else
				throw new Exception ("Cannot update '" + entity.GetType ().Name + "' entity. Not found in data store.");
		}
	}
}

