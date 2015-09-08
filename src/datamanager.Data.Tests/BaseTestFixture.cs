using System;
using NUnit.Framework;

namespace datamanager.Data.Tests
{
	public class BaseTestFixture
	{
		public BaseTestFixture ()
		{
		}

		[SetUp]
		public void Initialize()
		{
			DataConfig.Prefix += "-Test";
		}
	}
}

