using System;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataChecker
	{
		public DataManagerSettings Settings;

		public DataReader Reader;

		public DataChecker (DataReader reader, DataManagerSettings settings)
		{
			Settings = settings;
			Reader = reader;
		}

		public bool Exists(BaseEntity entity)
		{
			if (Settings.IsVerbose)
				Console.WriteLine ("Checking if entity exists: " + entity.GetType().Name);

			var foundEntity = Reader.Read(entity.GetType(), entity.Id);

			var exists = foundEntity != null;

			if (Settings.IsVerbose)
				Console.WriteLine ("  Exists: " + exists);

			return exists;
		}
	}
}

