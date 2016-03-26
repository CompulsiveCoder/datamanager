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

			var mockLinker = new MockDataLinker (
				Context.Settings,
				Context.Reader,
				Context.Saver,
				Context.Updater,
				Context.Checker,
				Context.EntityLinker
			);

			var saver = new DataSaver (
				Context.Settings,
				Context.TypeManager,
				Context.IdManager,
				Context.Keys,
				Context.Preparer,
				mockLinker,
				Context.Checker,
				Context.DataClient);

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

			var mockLinker = new MockDataLinker (
				Context.Settings,
				Context.Reader,
				Context.Saver,
				Context.Updater,
				Context.Checker,
				Context.EntityLinker
			);

			var saver = new DataSaver (
				Context.Settings,
				Context.TypeManager,
				Context.IdManager,
				Context.Keys,
				Context.Preparer,
				mockLinker,
				Context.Checker,
				Context.DataClient);

			// Save the entity
			saver.Save (entity);

			// Call the Save function again which should throw an exception
			saver.Save(entity);
		}
	}
}

