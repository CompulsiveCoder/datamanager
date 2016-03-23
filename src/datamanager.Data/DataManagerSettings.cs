using System;

namespace datamanager.Data
{
	public class DataManagerSettings
	{
		public string Prefix = "";

		public bool IsVerbose = false;

		public DataManagerSettings (string prefix)
		{
			Prefix = prefix;
		}

		public DataManagerSettings ()
		{
			Prefix = Guid.NewGuid ().ToString ();
		}
	}
}

