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
			Console.WriteLine ("Saving: " + entity.GetType ().Name);

			var key = new DataKeys ().GetKey (entity);
			var json = Data.Preparer.PrepareForStorage(entity).ToJson ();
			Data.Client.Set(key, json);

			Data.IdManager.Add (entity);

			Data.Linker.CommitLinks (entity);
		}
	}
}

