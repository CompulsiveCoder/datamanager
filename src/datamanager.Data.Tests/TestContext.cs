using System;
using datamanager.Entities;

namespace datamanager.Data.Tests
{
	public class TestContext
	{
		public MockRedisClientWrapper DataClient { get; set; }

		public DataManagerSettings Settings { get;set; }

		public DataTypeManager TypeManager { get; set; }

		public DataIdManager IdManager { get; set; }

		public DataKeys Keys { get; set; }

		public DataPreparer Preparer { get;set; }
		public DataLinker Linker { get;set; }
		public DataSaver Saver { get;set; }
		public DataUpdater Updater { get;set; }
		public DataDeleter Deleter { get;set; }
		public DataReader Reader { get;set; }
		public DataChecker Checker { get;set; }

		public EntityLinker EntityLinker { get; set; }

		public TestContext ()
		{
			DataClient = new MockRedisClientWrapper ();

			Settings = new DataManagerSettings ();
			Settings.IsVerbose = true;

			Keys = new DataKeys (Settings);

			IdManager = new DataIdManager (Keys, DataClient);
			TypeManager = new DataTypeManager (Keys, DataClient);

			EntityLinker = new EntityLinker ();

			Preparer = new DataPreparer (DataClient);
			Saver = new DataSaver (Settings, TypeManager, IdManager, Keys, Preparer, Linker, Checker, DataClient);
			Updater = new DataUpdater (Settings, Keys, Linker, Preparer, Checker, DataClient);
			Reader = new DataReader (TypeManager, IdManager, Keys, DataClient);
			Linker = new DataLinker (Settings, Reader, Saver, Updater, Checker, EntityLinker);
			Checker = new DataChecker (Reader, Settings);
		}
	}
}

