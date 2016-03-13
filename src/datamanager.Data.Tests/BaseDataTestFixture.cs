using System;
using NUnit.Framework;

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
	}
}

