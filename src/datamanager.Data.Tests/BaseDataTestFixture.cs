using System;
using NUnit.Framework;
using datamanager.Entities;
using datamanager.Data.Providers.Memory;

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

		public DataManager GetDataManager()
		{
            var data = new DataManager (new MemoryDataProvider());
			data.Settings.Prefix = "Test-" + Guid.NewGuid ().ToString ().Substring (0, 8);
            data.Settings.IsVerbose = true;
			return data;
		}
	}
}

