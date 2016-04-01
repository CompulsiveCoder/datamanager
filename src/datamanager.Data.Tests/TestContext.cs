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

			var preparer = new DataPreparer (DataClient);
			Preparer = preparer;

			var reader = new DataReader (TypeManager, IdManager, Keys, DataClient);
			Reader = reader;

			var checker = new DataChecker (reader, Settings);
			Checker = checker;

			var saver = new DataSaver (Settings, TypeManager, IdManager, Keys, preparer, null, checker, DataClient); // The linker argument is null because it needs to be set after it's created below
			Saver = saver;

			var updater = new DataUpdater (Settings, Keys, null, preparer, checker, DataClient); // The linker argument is null because it needs to be set after it's created below
			Updater = updater;

			var linker = new DataLinker (Settings, reader, saver, updater, checker, EntityLinker);
			Linker = linker;

			// TODO: Is there a way to avoid this messy hack?
			// Make sure the linker is set to the saver and updater
			saver.Linker = linker;
			updater.Linker = linker;
		}
	}
}

