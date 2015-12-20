using System;
using Newtonsoft.Json;

namespace datamanager.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleReferenceRight : BaseEntity
	{
		public int NumberValue = 0;

		[TwoWay("Right")]
		public ExampleReferenceLeft Left { get; set; }

		[TwoWay("OtherRight")]
		public ExampleReferenceLeft OtherLeft { get; set; }

		public ExampleReferenceRight ()
		{
		}
	}
}

