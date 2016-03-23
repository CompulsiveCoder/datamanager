using System;
using Newtonsoft.Json;
using datamanager.Entities;

namespace datamanager.Data
{
	public class BaseDataAdapter
	{
		public bool TestMode = false;

		// TODO: Remove if not needed
		//public DataManager Data;

		public BaseRedisClientWrapper Client;

		//public DataKeys Keys;

		/*public BaseDataAdapter ()
		{
			Data = new DataManager ();
			Keys = new DataKeys (Data.Settings);
		}

		public BaseDataAdapter(DataManager data)
		{
			Data = data;
			Keys = new DataKeys (Data.Settings);
		}*/

		public BaseDataAdapter(BaseRedisClientWrapper client)
		{
			Client = client;
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

