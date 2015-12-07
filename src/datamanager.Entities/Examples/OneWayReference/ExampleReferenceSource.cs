using System;

namespace datamanager.Entities
{
	public class ExampleReferenceSource : BaseEntity
	{
		public ExampleReferenceTarget Target { get; set; }

		public ExampleReferenceSource ()
		{
		}
	}
}

