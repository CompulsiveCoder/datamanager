using System;
using datamanager.Entities;

namespace datamanager.Data
{
	public class EntityNotFoundException : Exception
	{
		public BaseEntity Entity { get;set; }

		public EntityNotFoundException (BaseEntity entity) : base("'" + entity.GetType ().Name + "' entity not found in data store.")
		{
			Entity = entity;
		}
	}
}

