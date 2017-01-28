using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Unit
{
	[TestFixture(Category="Unit")]
	public class DataSaverUnitTestFixture : BaseDataTestFixture
	{
		[Test]
		public void Test_Save()
		{
			Console.WriteLine ("Preparing test");

			// Create the entity
			var entity = new SimpleEntity ();

            var data = GetDataManager ();

			var mockLinker = new MockDataLinker (
				data.Settings,
				data.Reader,
				data.Saver,
				data.Updater,
				data.Checker,
				data.EntityLinker
			);

			var saver = new DataSaver (
				data.Settings,
				data.TypeManager,
				data.IdManager,
				data.Keys,
				data.Preparer,
				mockLinker,
				data.Checker,
                data.Provider);

			Console.WriteLine ("Executing test");

			// Save the entity
			saver.Save (entity);
		}

		// TODO: Should this be moved to integration tests?
		[Test]
		[ExpectedException(typeof(EntityAlreadyExistsException))]
		public void Test_Save_EntityAlreadyExistsException()
		{
			// Create the entity
			var entity = new SimpleEntity ();

            var data = GetDataManager ();

			var mockLinker = new MockDataLinker (
				data.Settings,
				data.Reader,
				data.Saver,
				data.Updater,
				data.Checker,
				data.EntityLinker
			);

			var saver = new DataSaver (
				data.Settings,
				data.TypeManager,
				data.IdManager,
				data.Keys,
				data.Preparer,
				mockLinker,
				data.Checker,
				data.Provider);

			// Save the entity
			saver.Save (entity);

			// Call the Save function again which should throw an exception
			saver.Save(entity);
		}
	}
}

