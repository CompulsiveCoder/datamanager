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
	}
}

