using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Unit
{
	[TestFixture]
	public class DataSaverTestFixture
	{
		[Test]
		[ExpectedException(typeof(EntityAlreadyExistsException))]
		public void Test_Save_EntityAlreadyExistsException()
		{
			// Create the entity
			var entity = new SimpleEntity ();

			// Save the entity
			var data = new DataManager ();
			data.Save (entity);

			// Call the Save function again which should throw an exception
			data.Saver.Save(entity);
		}
	}
}

