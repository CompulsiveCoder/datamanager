using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests
{
	[TestFixture]
	public class DataLinkerTestFixture : BaseTestFixture
	{
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

			left.Right = new ExampleReferenceRight[]{right};

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
			var data = new DataManager();

			var left = new ExampleReferenceLeft ();
			var right = new ExampleReferenceRight ();

			left.Right = new ExampleReferenceRight[]{right};

			data.Save(left);

			right.Left = left;

			data.Save (right);

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

			data.Save(left);

			right.Left = left;

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

			left.Right = new ExampleReferenceRight[]{right};

			data.Save(left);

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

