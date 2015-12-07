using System;
using NUnit.Framework;

namespace datamanager.Entities.Tests
{
	[TestFixture]
	public class EntityLinkerTestFixture
	{
		[Test]
		public void Test_AddLink_OneWay_SingleEntity()
		{
			var source = new ExampleReferenceSource ();
			var target = new ExampleReferenceTarget ();

			var adder = new EntityLinker ();

			adder.AddLink (source, "Target", target);

			//Assert.IsNotNull (source.Target, "Link failed.");
			//Assert.IsNotNull (target.Left, "Reverse link failed.");
		}

		[Test]
		public void Test_AddLink_TwoWay_SingleEntity()
		{
			var left = new ExampleReferenceLeft ();
			var right = new ExampleReferenceRight ();

			var adder = new EntityLinker ();

			adder.AddLink (left, "Right", right);

			Assert.IsNotNull (left.Right, "Link failed.");
			Assert.IsNotNull (right.Left, "Reverse link failed.");
		}
	}
}

