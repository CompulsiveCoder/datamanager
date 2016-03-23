using System;
using Newtonsoft.Json;

namespace datamanager.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleInvoiceItem : BaseEntity
	{
		public string Description = "";
		public int Amount = 0;

		[TwoWay("Items")]
		public ExampleInvoice Invoice { get; set; }

		// TODO: Remove if not needed
		/*[TwoWay("OtherRight")]
		public ExampleReferenceLeft OtherLeft { get; set; }*/

		public ExampleInvoiceItem ()
		{
		}
	}
}

