using System;
using Sider;

namespace datamanager.Data.Providers.Redis
{
    public class RedisDataProvider : BaseDataProvider
    {
        public RedisClient Client { get;set; }

        public RedisDataProvider ()
        {
            Client = new RedisClient ();
        }

        public override string Get(string key)
        {
            return Client.Get (key);
        }

        public override string[] GetKeys ()
        {
            return Client.Keys("");
        }

        public override string[] GetKeys (string pattern)
        {
            return Client.Keys("*" + pattern + "*");
        }

        public override void Set (string key, string value)
        {
            Client.Set (key, value);
        }

        public override void Delete (string key)
        {
            Client.Del (key);
        }

        public override void DeleteAll ()
        {
            Client.FlushAll ();
        }

        public override void Quit()
        {
            Client.Quit ();
        }
    }
}

