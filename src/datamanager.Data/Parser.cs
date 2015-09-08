using System;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace datamanager.Data
{
	public class Parser
	{
		public Parser ()
		{
		}

		public T Parse<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}

