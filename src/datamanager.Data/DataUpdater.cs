using System;
using Sider;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataUpdater : BaseDataAdapter
	{
		public DataManagerSettings Settings;

		public DataKeys Keys;

		public DataChecker Checker;
		public DataLinker Linker;
		public DataPreparer Preparer;

		public BaseRedisClientWrapper Client;


		public DataUpdater (DataManagerSettings settings, DataKeys keys, DataLinker linker, DataPreparer preparer, DataChecker checker, BaseRedisClientWrapper client) : base(client)
		{
			Settings = settings;
			Keys = keys;
			Linker = linker;
			Preparer = preparer;
			Checker = checker;
		}

		public void Update(BaseEntity entity)
		{
			if (Checker.Exists (entity)) {
				if (Settings.IsVerbose)
					Console.WriteLine ("Updating: " + entity.GetType ().Name);

				Linker.CommitLinks (entity);

				var key = Keys.GetKey (entity);
				Client.Set (key, Preparer.PrepareForStorage (entity).ToJson ());
			}// else if (!Data.PendingSave.Contains(entity)) // TODO: Remove if not needed
				throw new EntityNotFoundException (entity);
		}
	}
}

