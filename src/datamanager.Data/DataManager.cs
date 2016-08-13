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

		public List<BaseEntity> PendingDelete = new List<BaseEntity>();

		public DataManagerSettings Settings = new DataManagerSettings();

		public DataManager ()
		{
			Construct ();
		}

		public DataManager(BaseRedisClientWrapper client)
		{
			Construct (client);
		}

		public void Construct()
		{
			Construct (new RedisClientWrapper ());
		}

		public void Construct(BaseRedisClientWrapper client)
		{
			Client = client;

			Keys = new DataKeys (Settings);

			TypeManager = new DataTypeManager (Keys, Client);
			IdManager = new DataIdManager (Keys, Client);

			EntityLinker = new EntityLinker ();


			var preparer = new DataPreparer (Client);
			Preparer = preparer;

			var reader = new DataReader (TypeManager, IdManager, Keys, client);
			Reader = reader;

			var lister = new DataLister (TypeManager, IdManager, reader, client);
			Lister = lister;

			var checker = new DataChecker (reader, Settings);
			Checker = checker;

			var saver = new DataSaver (Settings, TypeManager, IdManager, Keys, preparer, null, checker, client); // The linker argument is null because it needs to be set after it's created below
			Saver = saver;

			var updater = new DataUpdater (Settings, Keys, null, preparer, checker, client); // The linker argument is null because it needs to be set after it's created below
			Updater = updater;

			var linker = new DataLinker (Settings, reader, saver, updater, checker, EntityLinker);
			Linker = linker;

			var deleter = new DataDeleter (IdManager, Keys, linker, client);
			Deleter = deleter;

			// TODO: Is there a way to avoid this messy hack?
			// Make sure the linker is set to the saver and updater
			saver.Linker = linker;
			updater.Linker = linker;

			// TODO: Remove if not needed
			/*Preparer = new DataPreparer (Client);
			Reader = new DataReader (TypeManager, IdManager, Keys, Client);
			Checker = new DataChecker (Reader, Settings);
			Linker = new DataLinker (Settings, Reader, Saver, Updater, Checker, EntityLinker);
			Saver = new DataSaver (Settings, TypeManager, IdManager, Keys, Preparer, Linker, Checker, Client);
		
			Deleter = new DataDeleter (IdManager, Keys, Linker, Client);
			Updater = new DataUpdater (Settings, Keys, Linker, Preparer, Checker, Client);
			Lister = new DataLister (TypeManager, IdManager, Reader, Client);*/
		}

		public void Open()
		{
		}

		public void SaveOrUpdate(BaseEntity entity)
		{
			if (Settings.IsVerbose)
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

		// TODO: Remove if not needed
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

		// TODO: Remove if not needed
		/*public void Update(BaseEntity entity, bool saveLinkedEntities)
		{
			Save (entity);

			// TODO: Should linked entities be saved before or after?
			if (saveLinkedEntities)
				SaveLinkedEntities (entity);
		}*/
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
			if (Settings.IsVerbose)
				Console.WriteLine ("Committing pending entities");
			
			// TODO: Remove if not needed
			Saver.CommitPendingSaves ();

			Updater.CommitPendingUpdates ();

			// TODO: Move to deleter object
			CommitPendingDeletes ();
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

