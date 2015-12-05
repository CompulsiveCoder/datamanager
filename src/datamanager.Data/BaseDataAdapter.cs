using System;
using Newtonsoft.Json;
using datamanager.Entities;

namespace datamanager.Data
{
	public class BaseDataAdapter
	{
		public bool TestMode = false;

		public DataManager Data;

		public BaseDataAdapter ()
		{
			Data = new DataManager ();
		}

		public BaseDataAdapter(DataManager data)
		{
			Data = data;
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

