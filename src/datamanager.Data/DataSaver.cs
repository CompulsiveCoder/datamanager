using System;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using datamanager.Entities;
using System.Collections.Generic;
using datamanager.Data.Providers;

namespace datamanager.Data
{
	public class DataSaver : BaseDataAdapter
	{
		public DataManagerSettings Settings;
		public DataIdManager IdManager;
		public DataTypeManager TypeManager;
		public DataKeys Keys;
		public DataPreparer Preparer;
		public DataChecker Checker;
		public DataLinker Linker;

		public List<BaseEntity> PendingSave = new List<BaseEntity>();

        public DataSaver (DataManagerSettings settings, DataTypeManager typeManager, DataIdManager idManager, DataKeys keys, DataPreparer preparer, DataLinker linker, DataChecker checker, BaseDataProvider provider) : base (provider)
		{
			Settings = settings;
			IdManager = idManager;
			TypeManager = typeManager;
			Keys = keys;
			Preparer = preparer;
			Checker = checker;
			Linker = linker;
		}

		public void Save(BaseEntity entity)
		{
			Save (entity,  true);
		}

		public void Save(BaseEntity entity, bool commitLinks)
		{
			if (!Checker.Exists (entity)) {
				var entityType = entity.GetType ();

				if (Settings.IsVerbose)
					Console.WriteLine ("Saving: " + entityType.Name);

				TypeManager.EnsureExists (entityType);

				// Commit links before saving, otherwise it will fail
				if (commitLinks)
					Linker.CommitLinks (entity);

				InternalSave (entity);
			} else
				throw new EntityAlreadyExistsException (entity);
		}

		public void InternalSave(BaseEntity entity)
		{
			var key = Keys.GetKey (entity);
			var json = Preparer.PrepareForStorage (entity).ToJson ();
			Provider.Set (key, json);

			IdManager.Add (entity);
		}

		public void DelaySave(BaseEntity entity)
		{
			if (!PendingSave.Contains(entity))
				PendingSave.Add (entity);
		}

		public void CommitPendingSaves()
		{
			while (PendingSave.Count > 0)
			{
				var entity = PendingSave [0];
				if (!Checker.Exists (entity)) {
					Save (entity);
					PendingSave.RemoveAt (0);
				}
			}
		}
	}
}

