using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace datamanager.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleInvoice : BaseEntity
	{
		public int NumberValue = 0;

		[TwoWay("Invoice")]
		public ExampleInvoiceItem[] Items { get; set; }

		// TODO: Remove if not needed
		//[TwoWay("OtherLeft")]
		//public List<ExampleReferenceRight> OtherRight { get; set; }

		public ExampleInvoice ()
		{
		}

		public ExampleInvoice(params ExampleInvoiceItem[] items)
		{
			Items = items;
			foreach (var item in Items) {
				item.Invoice = this;
			}
		}
	}
}

