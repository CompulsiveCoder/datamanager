using System;
using System.Collections.Generic;

namespace datamanager.Data.Tests
{
	public class MockRedisClientWrapper : BaseRedisClientWrapper
	{
		public Dictionary<string, string> Data = new Dictionary<string, string>();

		public MockRedisClientWrapper ()
		{
		}

		public override string Get(string key)
		{
			if (Data.ContainsKey (key))
				return Data [key];
			else
				return String.Empty;
		}

		public override void Set (string key, string value)
		{
			Data[key] = value;
		}

		public override void Del (string key)
		{
			Data.Remove (key);
		}

		public override void FlushAll ()
		{
			Data = new Dictionary<string, string> ();
		}

		public override void Quit()
		{
			// not needed in mock redis client wrapper
		}
	}
}

