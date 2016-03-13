using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Unit
{
	[TestFixture]
	public class DataUpdaterTestFixture
	{
		[Test]
		[ExpectedException(typeof(EntityNotFoundException))]
		public void Test_Update_EntityNotFoundException()
		{
			// Create the entity
			var entity = new SimpleEntity ();

			// Call the Update function which should throw an exception because it hasn't been saved yet
			var data = new DataManager ();
			data.Updater.Update(entity);
		}
	}
}

