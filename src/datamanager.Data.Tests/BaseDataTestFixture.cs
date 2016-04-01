using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests
{
	public class BaseDataTestFixture
	{
		public BaseDataTestFixture ()
		{
		}

		[SetUp]
		public void Initialize()
		{
			Console.WriteLine ("Setting up test fixture " + this.GetType ().FullName);
		}

		public TestContext GetTestContext()
		{
			return new TestContext ();
		}

		public DataManager GetDataManager()
		{
			var data = new DataManager ();
			data.Settings.Prefix = "Test-" + Guid.NewGuid ().ToString ().Substring (0, 8);
			data.Client = new MockRedisClientWrapper ();
			return data;
		}
	}
}

