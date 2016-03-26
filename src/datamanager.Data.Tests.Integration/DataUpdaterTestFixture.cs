using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class DataUpdaterTestFixture : BaseDataIntegrationTestFixture
	{
		[Test]
		[ExpectedException(typeof(EntityNotFoundException))]
		public void Test_Update_EntityNotFoundException()
		{
			// Create the entity
			var entity = new SimpleEntity ();

			var data = GetDataManager ();

			// Call the Update function which should throw an exception because it hasn't been saved yet
			data.Updater.Update(entity);
		}
	}
}

