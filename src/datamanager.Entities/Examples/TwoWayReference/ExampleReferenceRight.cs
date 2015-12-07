using System;
using Newtonsoft.Json;

namespace datamanager.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleReferenceRight : BaseEntity
	{
		[TwoWay("Right")]
		public ExampleReferenceLeft Left { get; set; }

		public ExampleReferenceRight ()
		{
		}
	}
}

