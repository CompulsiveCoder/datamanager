using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests
{
	public class BaseDataTestFixture
	{
		public TestContext Context;

		public BaseDataTestFixture ()
		{
		}

		[SetUp]
		public void Initialize()
		{
			Console.WriteLine ("Setting up test fixture " + this.GetType ().FullName);
			Context = new TestContext ();
		}
	}
}

