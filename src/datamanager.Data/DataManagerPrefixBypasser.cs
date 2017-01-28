using System;
using datamanager.Data.Providers;
using System.Linq;

namespace datamanager.Data
{
    public class DataManagerPrefixBypasser
    {
        public DataManagerSettings Settings { get;set; }
        
        public BaseDataProvider Provider { get; set; }

        public DataManagerPrefixBypasser (DataManagerSettings settings, BaseDataProvider provider)
        {
            Settings = settings;
            Provider = provider;
        }

        public string[] FindKeysFromType(Type entityType)
        {
            return FindKeysFromType (entityType.Name);
        }

        public string[] FindKeysFromType(string typeName)
        {
            var pattern = "-" + typeName + "-";

            if (Settings.IsVerbose) {
                Console.WriteLine ("Finding keys from type");
                Console.WriteLine ("  Type name: " + typeName);
                Console.WriteLine ("  Pattern: " + pattern);
            }

            var keysFound = Provider.GetKeys (pattern);

            if (Settings.IsVerbose) {
                Console.WriteLine ("  Keys found: ");
                foreach (var key in keysFound)
                    Console.WriteLine ("    " + key);
                if (keysFound.Length == 0)
                    Console.WriteLine ("    No keys found");
                Console.WriteLine ("  Total keys found: " + keysFound.Length);
            }

            return keysFound;
        }
    }
}

