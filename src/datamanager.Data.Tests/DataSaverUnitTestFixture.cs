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

			var context = GetTestContext ();

			var mockLinker = new MockDataLinker (
				context.Settings,
				context.Reader,
				context.Saver,
				context.Updater,
				context.Checker,
				context.EntityLinker
			);

			var saver = new DataSaver (
				context.Settings,
				context.TypeManager,
				context.IdManager,
				context.Keys,
				context.Preparer,
				mockLinker,
				context.Checker,
				context.DataClient);

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

			var context = GetTestContext ();

			var mockLinker = new MockDataLinker (
				context.Settings,
				context.Reader,
				context.Saver,
				context.Updater,
				context.Checker,
				context.EntityLinker
			);

			var saver = new DataSaver (
				context.Settings,
				context.TypeManager,
				context.IdManager,
				context.Keys,
				context.Preparer,
				mockLinker,
				context.Checker,
				context.DataClient);

			// Save the entity
			saver.Save (entity);

			// Call the Save function again which should throw an exception
			saver.Save(entity);
		}
	}
}

