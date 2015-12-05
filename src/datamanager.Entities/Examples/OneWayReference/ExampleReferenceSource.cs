using System;

namespace datamanager.Entities
{
	public class ExampleReferenceSource : BaseEntity
	{
		public EntityLink Target { get; set; }

		public ExampleReferenceSource ()
		{
		}
	}
}

