using System;

namespace datamanager.Entities
{
	[Serializable]
	public class SimpleEntity : BaseEntity
	{
		public int NumberValue { get; set; }

		public SimpleEntity ()
		{
		}
	}
}

