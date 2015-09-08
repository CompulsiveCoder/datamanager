using System;
using Newtonsoft.Json;

namespace datamanager.Entities
{
	public class BaseEntity
	{
		public string Id;

		public BaseEntity ()
		{
			Id = Guid.NewGuid ().ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject (this);
		}
	}
}

