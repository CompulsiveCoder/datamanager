using System;
using datamanager.Entities;
using datamanager.Data.Providers;

namespace datamanager.Data
{
	public class DataPreparer : BaseDataAdapter
	{
        public DataPreparer (BaseDataProvider provider) : base(provider)
		{
		}

		public BaseEntity PrepareForStorage(BaseEntity entity)
		{
            // TODO: Implement clean up of entity if needed

			return entity;
		}
	}
}

