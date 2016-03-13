using System;
using Sider;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataSaver : BaseDataAdapter
	{
		public DataSaver()
		{
		}

		public DataSaver (DataManager dataManager) : base (dataManager)
		{
		}

		public void Save(BaseEntity entity)
		{
			if (!Data.Exists (entity)) {
				if (Data.IsVerbose)
					Console.WriteLine ("Saving: " + entity.GetType ().Name);

				// Commit links before saving, otherwise it will fail
				Data.Linker.CommitLinks (entity);

				var key = Keys.GetKey (entity);
				var json = Data.Preparer.PrepareForStorage (entity).ToJson ();
				Data.Client.Set (key, json);

				Data.IdManager.Add (entity);

			} else
				throw new EntityAlreadyExistsException (entity);
		}
	}
}

