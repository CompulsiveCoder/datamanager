using System;

namespace datamanager.Entities
{
	[Serializable]
	public class ExampleReferenceRight : BaseEntity
	{
		[TwoWay("Right")]
		public EntityLink Left { get; set; }

		public ExampleReferenceRight ()
		{
		}
	}
}

