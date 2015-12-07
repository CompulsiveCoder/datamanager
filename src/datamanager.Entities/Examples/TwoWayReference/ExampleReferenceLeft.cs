using System;
using Newtonsoft.Json;

namespace datamanager.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleReferenceLeft : BaseEntity
	{
		[TwoWay("Left")]
		public ExampleReferenceRight[] Right { get; set; }

		public ExampleReferenceLeft ()
		{
		}
	}
}

