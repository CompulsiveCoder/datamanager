using System;
using Newtonsoft.Json;
using datamanager.Entities;
using datamanager.Data.Providers;

namespace datamanager.Data
{
	public class BaseDataAdapter
	{
        public BaseDataProvider Provider;

		public BaseDataAdapter(BaseDataProvider provider)
		{
			Provider = provider;
		}

		public string EntityToJson(BaseEntity entity)
		{
			return JsonConvert.SerializeObject(entity);
		}

		public string ArrayToJson(BaseEntity[] entities)
		{
			return JsonConvert.SerializeObject(entities);
		}

		public T JsonToEntity<T>(string json)
		{
			return JsonConvert.DeserializeObject<T> (json);
		}

		public T[] JsonToArray<T>(string json)
		{
			return JsonConvert.DeserializeObject<T[]> (json);
		}
	}
}

