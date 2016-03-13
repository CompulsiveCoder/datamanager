using System;
using datamanager.Entities;

namespace datamanager.Data
{
	public class EntityAlreadyExistsException : Exception
	{
		public EntityAlreadyExistsException (BaseEntity entity) : base("'" + entity.GetType ().Name + "' with the same ID already exists in the data store.")
		{
		}
	}
}

