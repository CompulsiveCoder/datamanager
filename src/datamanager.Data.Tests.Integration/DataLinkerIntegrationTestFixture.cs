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
		// TODO: Remove if not needed
		//[Test]
		public void Test_CommitLinks_NonSavedEntity()
		{
			var left = new ExampleInvoice ();
			var right = new ExampleInvoiceItem ();

			right.Invoice = left;

			// Try to save the "right" object without first saving the "left" object. It should throw an exception because it can't sync with
			// a non-existent entity
			Data.Linker.CommitLinks(right);
		}


		// TODO: Remove if not needed
		//[Test]
		public void Test_TwoWayReference_Add()
		{
			var data = new DataManager();

			var invoice = new ExampleInvoice ();

			data.Save(invoice);

			var invoiceItem = new ExampleInvoiceItem ();

			invoiceItem.AddLink("Invoice", invoice);

			data.Save (invoiceItem);

			// The "left.Right" property should now contain a link to the "right" object
			Assert.IsNotNull (invoice.Items, "Linker failed to add the link to the other entity.");
		}

		// TODO: Remove if not needed
		//[Test]
		public void Test_TwoWayReference_RemoveReverseLinkOnUpdate()
		{
			var data = new DataManager();

			var invoice = new ExampleInvoice ();
			var invoiceItem = new ExampleInvoiceItem ();

			data.Save(invoice);

			invoiceItem.Invoice = invoice;

			data.Save (invoiceItem);

			invoiceItem.Invoice = null;

			data.Update(invoiceItem);

			var newInvoice = data.Get<ExampleInvoice> (invoice.Id);

			Assert.AreEqual(0, newInvoice.Items.Length, "Linker failed to remove the link.");
		}

		// TODO: Remove if not needed
		//[Test]
		public void Test_TwoWayReference_RemoveOnDelete()
		{

			Console.WriteLine ("");
			Console.WriteLine ("Preparing test...");
			Console.WriteLine ("");

			var data = new DataManager();
			data.IsVerbose = true;

			data.WriteSummary ();

			var invoiceItem = new ExampleInvoiceItem ();
			var invoice = new ExampleInvoice (invoiceItem);

			data.Save(invoice, true);

			data.WriteSummary ();

			Console.WriteLine ("");
			Console.WriteLine ("Executing test code...");
			Console.WriteLine ("");

			data.Delete(invoice);

			data.WriteSummary ();

			var newInvoice = data.Get<ExampleInvoice> (invoice.Id);

			Assert.AreEqual(0, newInvoice.Items.Length, "Linker failed to remove the link.");
		}

		// TODO: Remove if not needed
		//[Test]
		public void Test_SaveLinkedEntities()
		{
			var data = new DataManager();

			var invoice = new ExampleInvoice ();
			var invoiceItem = new ExampleInvoiceItem ();

			invoice.Items = new ExampleInvoiceItem[]{invoiceItem};

			//data.Save(left);

			//right.Left = left;

			data.SaveLinkedEntities (invoice);


			var foundRight = data.Get<ExampleInvoiceItem> (invoiceItem.Id);

			// The "right" object should now be found in the data store
			Assert.IsNotNull (foundRight, "Linker failed to save the other entity.");
		}


		// TODO: Remove if not needed
		//[Test]
		public void Test_UpdateLinkedEntities()
		{
			var data = new DataManager();

			var left = new ExampleInvoice ();
			var right = new ExampleInvoiceItem ();


			data.Save(left);

			//left.Right = new ExampleReferenceRight[]{right}; // TODO: Remove. This shouldn't be needed

			right.Invoice = left;

			data.Save (right);

			right.Amount = 2;

			data.UpdateLinkedEntities (left);

			var foundRight = data.Get<ExampleInvoiceItem> (right.Id);

			// The "right" object should have been updated in the data store
			Assert.AreEqual(2, foundRight.Amount, "Linker failed to update the other entity.");
		}
	}
}

