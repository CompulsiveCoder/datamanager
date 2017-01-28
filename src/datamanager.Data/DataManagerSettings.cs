using System;

namespace datamanager.Data
{
	public class DataManagerSettings
    {
        public bool IsVerbose = false;

        public DirectoryContext Location;

        public string Prefix { get;set; }

        public DataManagerSettings (string dataDirectory)
        {
            Location = new DirectoryContext (dataDirectory);
        }

        public DataManagerSettings ()
        {
            Location = new DirectoryContext (Environment.CurrentDirectory);
        }

        static public DataManagerSettings Verbose
        {
            get {
                var settings = new DataManagerSettings ();
                settings.IsVerbose = true;
                return settings;
            }
        }

        static public DataManagerSettings CreateVerbose(string dataDirectory)
        {
            var settings = new DataManagerSettings (dataDirectory);
            settings.IsVerbose = true;
            return settings;
        }
    }
}

