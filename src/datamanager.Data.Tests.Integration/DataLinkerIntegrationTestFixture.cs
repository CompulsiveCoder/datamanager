using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Integration
{
	[TestFixture]
	public class DataLinkerIntegrationTestFixture : BaseDataIntegrationTestFixture
	{

		/// <summary>
		/// Ensure that an exception is thrown when a linked entity hasn't been saved yet. This is necessary because the linker cannot synchronise
		/// links with an entity that isn't in the data store.
		/// // TODO: Add a way to disable this check
		/// </summary>
		[Test]
		public void Test_CommitLinks_NonSavedEntity()
		{
			var left = new ExampleReferenceLeft ();
			var right = new ExampleReferenceRight ();

			right.Left = left;

			// Try to save the "right" object without first saving the "left" object. It should throw an exception because it can't sync with
			// a non-existent entity
			Data.Linker.CommitLinks(right);
		}


		[Test]
		public void Test_TwoWayReference_Add()
		{
			var data = new DataManager();

			var left = new ExampleReferenceLeft ();

			data.Save(left);

			var right = new ExampleReferenceRight ();

			right.AddLink("Left", left);

			data.Save (right);

			// The "left.Right" property should now contain a link to the "right" object
			Assert.IsNotNull (left.Right, "Linker failed to add the link to the other entity.");
		}

		[Test]
		public void Test_TwoWayReference_RemoveReverseLinkOnUpdate()
		{
			var data = new DataManager();

			var left = new ExampleReferenceLeft ();
			var right = new ExampleReferenceRight ();

			// left.Right = new ExampleReferenceRight[]{right};// TODO: Remove if not needed

			data.Save(left);

			right.Left = left;

			data.Save (right);

			right.Left = null;

			data.Update(right);

			var newLeft = data.Get<ExampleReferenceLeft> (left.Id);

			// The "left.Right" property should now be empty
			Assert.IsEmpty (newLeft.Right, "Linker failed to remove the link.");
		}

		[Test]
		public void Test_TwoWayReference_RemoveOnDelete()
		{

			Console.WriteLine ("");
			Console.WriteLine ("Preparing test...");
			Console.WriteLine ("");

			var data = new DataManager();

			var left = new ExampleReferenceLeft ();
			var right = new ExampleReferenceRight ();

			data.Save(left);

			right.Left = left;

			data.Save (right);

			Console.WriteLine ("");
			Console.WriteLine ("Executing test code...");
			Console.WriteLine ("");

			data.Delete(right);

			var newLeft = data.Get<ExampleReferenceLeft> (left.Id);

			// The "left.Right" property should now be empty
			Assert.IsEmpty (newLeft.Right, "Linker failed to remove the link.");
		}

		[Test]
		public void Test_SaveLinkedEntities()
		{
			var data = new DataManager();

			var left = new ExampleReferenceLeft ();
			var right = new ExampleReferenceRight ();

			left.Right = new ExampleReferenceRight[]{right};

			//data.Save(left);

			//right.Left = left;

			data.SaveLinkedEntities (left);


			var foundRight = data.Get<ExampleReferenceRight> (right.Id);

			// The "right" object should now be found in the data store
			Assert.IsNotNull (foundRight, "Linker failed to save the other entity.");
		}


		[Test]
		public void Test_UpdateLinkedEntities()
		{
			var data = new DataManager();

			var left = new ExampleReferenceLeft ();
			var right = new ExampleReferenceRight ();


			data.Save(left);

			//left.Right = new ExampleReferenceRight[]{right}; // TODO: Remove. This shouldn't be needed

			right.Left = left;

			data.Save (right);

			right.NumberValue = 2;

			data.UpdateLinkedEntities (left);

			var foundRight = data.Get<ExampleReferenceRight> (right.Id);

			// The "right" object should have been updated in the data store
			Assert.AreEqual(2, foundRight.NumberValue, "Linker failed to update the other entity.");
		}
	}
}

