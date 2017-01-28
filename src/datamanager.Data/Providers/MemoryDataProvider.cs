using System;
using System.Collections.Generic;
using System.Linq;

namespace datamanager.Data.Providers.Memory
{

    public class MemoryDataProvider : BaseDataProvider
    {
        public Dictionary<string, string> Data = new Dictionary<string, string>();

        public MemoryDataProvider ()
        {
        }

        public override string Get(string key)
        {
            if (Data.ContainsKey (key))
                return Data [key];
            else
                return String.Empty;
        }

        public override string[] GetKeys ()
        {
            return (from k in Data.Keys
                             select k.ToString ()).ToArray ();
        }

        public override string[] GetKeys (string pattern)
        {
            return (from k in Data.Keys
                where k.Contains(pattern) 
                select k.ToString ()).ToArray ();
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

