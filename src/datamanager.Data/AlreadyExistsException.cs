using System;

namespace datamanager.Data
{
	public class AlreadyExistsException : Exception
	{
		public AlreadyExistsException () : base("The entity already exists.")
		{
		}
	}
}

