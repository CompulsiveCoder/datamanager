using System;
using datamanager.Data;
using datamanager.Entities;
using Sider;
using System.Collections.Generic;

namespace datamanager.Data
{
	public class DataManager : IDisposable
	{
		public DataTypeManager TypeManager;
		public DataIdManager IdManager;

		public DataKeys Keys;

		public DataPreparer Preparer;

		public DataSaver Saver;
		public DataDeleter Deleter;
		public DataUpdater Updater;
		public DataReader Reader;
		public DataLister Lister;
		public DataLinker Linker;
		public DataChecker Checker;

		public EntityLinker EntityLinker;

		public BaseRedisClientWrapper Client;

		public List<BaseEntity> PendingSave = new List<BaseEntity>();
		public List<BaseEntity> PendingUpdate = new List<BaseEntity>();
		public List<BaseEntity> PendingDelete = new List<BaseEntity>();

		public DataManagerSettings Settings = new DataManagerSettings();

		public bool IsVerbose = true;

		public DataManager ()
		{
			Construct ();
		}

		public DataManager (string prefix)
		{
			Settings.Prefix = prefix;
			Construct ();
		}

		public DataManager (DataManagerSettings settings)
		{
			Settings = settings;
			Construct ();
		}

		public void Construct()
		{
			Keys = new DataKeys (Settings);

			Open ();

			TypeManager = new DataTypeManager (Keys, Client);
			IdManager = new DataIdManager (Keys, Client);
			//Preparer = new DataPreparer (this);

			EntityLinker = new EntityLinker ();

			Preparer = new DataPreparer (Client);
			Reader = new DataReader (TypeManager, IdManager, Keys, Client);
			Checker = new DataChecker (Reader, Settings);
			Linker = new DataLinker (Settings, Reader, Saver, Updater, Checker, EntityLinker);
			Saver = new DataSaver (Settings, TypeManager, IdManager, Keys, Preparer, Linker, Checker, Client);
		
			Deleter = new DataDeleter (IdManager, Keys, Linker, Client);
			Updater = new DataUpdater (Settings, Keys, Linker, Preparer, Checker, Client);
			Lister = new DataLister (TypeManager, IdManager, Reader, Client);
		}

		public void Open()
		{
			Client = new RedisClientWrapper ();
		}

		public void SaveOrUpdate(BaseEntity entity)
		{
			if (IsVerbose)
				Console.WriteLine ("Save/update");
			
			if (Exists (entity))
				Update (entity);
			else
				Save (entity);
		}

		public void Save(BaseEntity[] entities)
		{
			foreach (var entity in entities) {
				Save (entity);
			}
		}

		public void Save(BaseEntity entity)
		{
			Save (entity, false);
		}

		public void Save(BaseEntity entity, bool saveLinkedEntities)
		{
			Saver.Save (entity, saveLinkedEntities);

			// TODO: Remove if not needed
			CommitPending ();
		}

		/*public void Save(BaseEntity entity, bool saveLinkedEntities)
		{
			Save (entity);

			// TODO: Should linked entities be saved before or after?
			if (saveLinkedEntities)
				SaveLinkedEntities (entity);
		}*/


		public void Update(BaseEntity[] entities)
		{
			foreach (var entity in entities) {
				Update (entity);
			}
		}

		public void Update(BaseEntity entity)
		{
			Updater.Update (entity);

			CommitPending ();
		}

		/*public void Update(BaseEntity entity, bool saveLinkedEntities)
		{
			Save (entity);

			// TODO: Should linked entities be saved before or after?
			if (saveLinkedEntities)
				SaveLinkedEntities (entity);
		}*/

		public void DelayUpdate(BaseEntity entity)
		{
			if (!PendingUpdate.Contains(entity))
				PendingUpdate.Add (entity);
		}

		public void DelaySave(BaseEntity entity)
		{
			if (!PendingSave.Contains(entity))
				PendingSave.Add (entity);
		}

		public void Delete(BaseEntity entity)
		{
			Deleter.Delete (entity);

			CommitPending ();
		}

		public void DelayDelete(BaseEntity entity)
		{
			if (!PendingDelete.Contains(entity))
				PendingDelete.Add (entity);
		}

		public void CommitPending()
		{
			if (IsVerbose)
				Console.WriteLine ("Committing pending entities");
			
			// TODO: Remove if not needed
			//CommitPendingSaves ();

			CommitPendingUpdates ();

			CommitPendingDeletes ();
		}

		// TODO: Remove if not needed
		public void CommitPendingSaves()
		{
			while (PendingSave.Count > 0)
			{
				var entity = PendingSave [0];
				if (!Exists (entity)) {
					Saver.Save (entity);
					PendingSave.RemoveAt (0);
				}
			}
		}

		public void CommitPendingUpdates()
		{
			while (PendingUpdate.Count > 0)
			{
				try
				{
					var entity = PendingUpdate[0];
					if (Exists(entity))
					{
						Updater.Update (entity);
						PendingUpdate.RemoveAt (0);
					}
				}
				catch (EntityNotFoundException ex) {
					// TODO: Check if this exception should be thrown
					throw new UnsavedLinksException (ex.Entity);
				}
			}
		}

		// TODO: Should delayed deletion be removed? It's not currently being used by the data linker.
		public void CommitPendingDeletes()
		{
			while (PendingDelete.Count > 0)
			{
				Deleter.Delete (PendingDelete[0]);
				PendingDelete.RemoveAt (0);
			}
		}

		public T Get<T>(string id)
		{
			return Reader.Read<T> (id);
		}

		public T[] Get<T>()
		{
			return Lister.Get<T> ();
		}

		public BaseEntity[] GetAll()
		{
			return Lister.GetAll();
		}

		public BaseEntity Get(string entityTypeName, string entityId)
		{
			return Reader.Read (entityTypeName, entityId);
		}

		public BaseEntity Get(Type entityType, string entityId)
		{
			return Reader.Read (entityType, entityId);
		}

		public Type GetType(string typeName)
		{
			return TypeManager.GetType (typeName);
		}

		public int Count(string typeName)
		{
			return Lister.Get (typeName).Length;
		}

		public bool Exists(BaseEntity entity)
		{
			return Checker.Exists (entity);
		}

		public bool TypeExists(string typeName)
		{
			return TypeManager.Exists (typeName);
		}

		public void SaveLinkedEntities(BaseEntity entity)
		{
			Linker.SaveLinkedEntities (entity);
		}

		public void UpdateLinkedEntities(BaseEntity entity)
		{
			Linker.UpdateLinkedEntities (entity);
		}

		public void WriteSummary()
		{
			Console.WriteLine ("");
			Console.WriteLine ("Redis data summary...");

			var types = TypeManager.GetTypes ();
			if (types.Count == 0)
				Console.WriteLine ("  [empty]");
			else {
				foreach (var typeName in types.Keys) {
					Console.WriteLine (typeName + ": " + Count (typeName));
				}
			}
			Console.WriteLine ("");
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			Client.Quit ();
		}

		#endregion
	}
}

