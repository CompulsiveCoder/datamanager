using System;
using datamanager.Entities;

namespace datamanager.Data.Tests.ProfilingConsole
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            var dataManager = new DataManager();

            for (int i = 0; i < 500; i++) {
                var item = new ExampleInvoiceItem ();

                var invoice = new ExampleInvoice ();

                invoice.Items = new ExampleInvoiceItem[]{ item };

                dataManager.Save (invoice);
                dataManager.SaveLinkedEntities (invoice);
            }
        }
    }
}
