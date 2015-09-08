using System;
using Sider;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using datamanager.Entities;

namespace datamanager.Data
{
	public class EntitySaver : BaseDataAdapter
	{
		public void Save(BaseEntity entity)
		{
			var client = new RedisClient();
			var key = new EntityKeys ().GetKey (entity);
			var json = entity.ToJson ();
			client.Set(key, json);

			var idManager = new DataIdManager ();
			idManager.Add (entity);
		}
	}
}

