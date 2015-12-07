using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace datamanager.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleReferenceLeft : BaseEntity
	{
		[TwoWay("Left")]
		public ExampleReferenceRight[] Right { get; set; }

		[TwoWay("OtherLeft")]
		public List<ExampleReferenceRight> OtherRight { get; set; }

		public ExampleReferenceLeft ()
		{
		}
	}
}

