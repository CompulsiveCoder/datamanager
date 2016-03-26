using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class DataSaverIntegrationTestFixture : BaseDataIntegrationTestFixture
	{
		[Test]
		public void Test_Save()
		{
			Console.WriteLine ("Preparing test");

			// Create the entity
			var entity = new SimpleEntity ();

			Console.WriteLine ("Executing test");

			// Save the entity
			Data.Save (entity);
		}

		[Test]
		[ExpectedException(typeof(EntityAlreadyExistsException))]
		public void Test_Save_EntityAlreadyExistsException()
		{
			// Create the entity
			var entity = new SimpleEntity ();

			// Save the entity
			Data.Save (entity);

			// Call the Save function again which should throw an exception
			Data.Save(entity);
		}
	}
}

