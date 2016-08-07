using System;

namespace datamanager.Entities
{
    [Serializable]
	public class ExampleReferenceSource : BaseEntity
	{
		public ExampleReferenceTarget Target { get; set; }

		public ExampleReferenceSource ()
		{
		}
	}
}

