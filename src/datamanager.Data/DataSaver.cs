using System;
using Sider;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using datamanager.Entities;
using System.Collections.Generic;

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

		public DataSaver (DataManagerSettings settings, DataTypeManager typeManager, DataIdManager idManager, DataKeys keys, DataPreparer preparer, DataLinker linker, DataChecker checker, BaseRedisClientWrapper client) : base (client)
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

				// TODO: Remove if not needed
				//throw new NotImplementedException ();

				// TODO: Remove if not needed
				// Add to the "pending save" list so the linker knows not to throw an error when it's not already found
				//PendingSave.Add(entity);

				TypeManager.EnsureExists (entityType);

				//if (saveLinkedEntities)
				//	Linker.SaveLinkedEntities (entity);

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
			Client.Set (key, json);

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

