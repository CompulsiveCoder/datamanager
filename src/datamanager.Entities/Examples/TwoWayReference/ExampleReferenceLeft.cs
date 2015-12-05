using System;

namespace datamanager.Entities
{
	[Serializable]
	public class ExampleReferenceLeft : BaseEntity
	{
		[TwoWay("Left")]
		public EntityLink[] Right { get; set; }

		public ExampleReferenceLeft ()
		{
		}
	}
}

