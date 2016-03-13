using System;
using datamanager.Data;
using datamanager.Entities;
using Sider;
using System.Collections.Generic;

namespace datamanager.Data
{
	public class DataManager : IDisposable
	{
		public DataIdManager IdManager;
		public DataPreparer Preparer;

		public DataSaver Saver;
		public DataDeleter Deleter;
		public DataUpdater Updater;
		public DataReader Reader;
		public DataLister Lister;
		public DataLinker Linker;

		public RedisClient Client;

		public List<BaseEntity> PendingUpdate = new List<BaseEntity>();
		public List<BaseEntity> PendingDelete = new List<BaseEntity>();

		public string Prefix { get;set; }

		public bool IsVerbose = true;

		public DataManager ()
		{
			Construct ();
		}

		public void Construct()
		{
			IdManager = new DataIdManager (this);
			Preparer = new DataPreparer (this);

			Saver = new DataSaver (this);
			Deleter = new DataDeleter (this);
			Updater = new DataUpdater (this);
			Reader = new DataReader (this);
			Lister = new DataLister (this);
			Linker = new DataLinker (this);

			Open ();
		}

		public void Open()
		{
			Client = new RedisClient ();
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
			Saver.Save (entity);

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
			CommitPendingUpdates ();

			CommitPendingDeletes ();
		}

		public void CommitPendingUpdates()
		{
			while (PendingUpdate.Count > 0)
			{
				try
				{
					Updater.Update (PendingUpdate[0]);
					PendingUpdate.RemoveAt (0);
				}
				catch (EntityNotFoundException ex) {
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

		public BaseEntity Get(Type entityType, string entityId)
		{
			return Reader.Read (entityType, entityId);
		}

		public bool Exists(BaseEntity entity)
		{
			var foundEntity = Get(entity.GetType(), entity.Id);

			var exists = foundEntity != null;

			if (IsVerbose)
				Console.WriteLine ("Exists: " + exists);

			return exists;
		}

		public void SaveLinkedEntities(BaseEntity entity)
		{
			Linker.SaveLinkedEntities (entity);
		}

		public void UpdateLinkedEntities(BaseEntity entity)
		{
			Linker.UpdateLinkedEntities (entity);
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			Client.Quit ();
		}

		#endregion
	}
}

