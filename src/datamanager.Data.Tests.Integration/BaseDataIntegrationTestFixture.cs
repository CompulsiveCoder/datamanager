using System;
using NUnit.Framework;

namespace datamanager.Data.Tests
{
	public class BaseDataIntegrationTestFixture
	{
		public DataManager Data;

		public bool ResetDataOnTearDown = true;

		public BaseDataIntegrationTestFixture ()
		{
		}

		[SetUp]
		public void Initialize()
		{
			Console.WriteLine ("Setting up test fixture " + this.GetType ().FullName);

			Data = new DataManager ();
		}

		[TearDown]
		public void Dispose()
		{
			if (ResetDataOnTearDown)
				Data.Client.FlushAll ();
		}
	}
}

