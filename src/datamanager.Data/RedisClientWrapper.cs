using System;
using Sider;

namespace datamanager.Data
{
	public class RedisClientWrapper : BaseRedisClientWrapper
	{
		public RedisClient Client { get;set; }

		public RedisClientWrapper ()
		{
			Client = new RedisClient ();
		}

		public override string Get(string key)
		{
			return Client.Get (key);
		}

		public override void Set (string key, string value)
		{
			Client.Set (key, value);
		}

		public override void Del (string key)
		{
			Client.Del (key);
		}

		public override void FlushAll ()
		{
			Client.FlushAll ();
		}

		public override void Quit()
		{
			Client.Quit ();
		}
	}
}

