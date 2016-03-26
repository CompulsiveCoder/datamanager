using System;
using NUnit.Framework;

namespace datamanager.Data.Tests
{
	public abstract class BaseDataIntegrationTestFixture
	{
		// TODO: Remove if not needed
		private DataManager data;

		public bool ResetDataOnTearDown = true;

		public BaseDataIntegrationTestFixture ()
		{
		}

		[SetUp]
		public void Initialize()
		{
			Console.WriteLine ("Setting up test fixture " + this.GetType ().FullName);
		}

		[TearDown]
		public void Dispose()
		{
			if (data != null && ResetDataOnTearDown)
				data.Client.FlushAll ();
		}

		public DataManager GetDataManager()
		{
			data = new DataManager (new MockRedisClientWrapper ());

			data.Settings.Prefix = "Test-" + Guid.NewGuid ().ToString ().Substring (0, 8);
			data.Settings.IsVerbose = true;

			return data;
		}
	}
}

