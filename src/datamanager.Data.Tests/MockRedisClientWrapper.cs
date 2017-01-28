using System;
using System.Collections.Generic;
using datamanager.Data.Providers;

namespace datamanager.Data.Tests
{
    public class MockRedisClientWrapper : BaseDataProvider
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

		public override void Delete (string key)
		{
			Data.Remove (key);
		}

		public override void DeleteAll ()
		{
			Data = new Dictionary<string, string> ();
		}

		public override void Quit()
		{
			// not needed in mock redis client wrapper
		}
	}
}

