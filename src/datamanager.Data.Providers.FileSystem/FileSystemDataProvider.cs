using System;
using System.IO;

namespace datamanager.Data.Providers.FileSystem
{
    public class FileSystemDataProvider : BaseDataProvider
    {
        public FileSystemDataProvider ()
        {
            throw new NotImplementedException ();
        }

        public override string Get(string key)
        {
            var filePath = CreateFilePath (key);

            // TODO: Remove or reimplement
           //if (!File.Exists (filePath)) {
           //     if (Settings.IsVerbose)
           //         Console.WriteLine (entityType.FullName + " entity file not found: " + filePath);
           //     return null;
           // }

            var json = File.ReadAllText(filePath);

            // TODO: Remove or reimplement
            /*if (String.IsNullOrEmpty (json)) {
                if (Settings.IsVerbose)
                    Console.WriteLine ("Entity file empty: " + entityType.FullName);
                return null;
            }*/

            return json;
        }

        public override string[] GetKeys ()
        {
            throw new NotImplementedException ();
        }

        public override string[] GetKeys (string pattern)
        {
            throw new NotImplementedException ();
        }

        public override void Set (string key, string value)
        {
            var filePath = CreateFilePath(key);

            // TODO: Remove or reimplement
            //if (Settings.IsVerbose)
            //    Console.WriteLine ("  " + filePath);

            File.WriteAllText (filePath, value);
        }

        public override void Delete (string key)
        {
            var filePath = CreateFilePath (key);

            File.Delete (filePath);
        }

        public override void DeleteAll ()
        {
            throw new NotImplementedException ();
            //Client.FlushAll ();
        }

        public override void Quit()
        {
            throw new NotImplementedException ();
            //Client.Quit ();
        }

        public string CreateFilePath(string key)
        {
            return key;
        }
    }
}
   