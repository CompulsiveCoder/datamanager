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

		public bool IsVerbose = false;

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
			if (Exists (entity))
				Update (entity);
			else
				Save (entity);
		}

		public void Save(BaseEntity entity)
		{
			Saver.Save (entity);
		}

		public void Update(BaseEntity entity)
		{
			Updater.Update (entity);

			while (PendingUpdate.Count > 0)
			{
				Updater.Update (PendingUpdate[0]);
				PendingUpdate.RemoveAt (0);
			}
		}

		public void DelayUpdate(BaseEntity entity)
		{
			if (!PendingUpdate.Contains(entity))
				PendingUpdate.Add (entity);
		}

		public void Delete(BaseEntity entity)
		{
			Deleter.Delete (entity);

			while (PendingDelete.Count > 0)
			{
				Deleter.Delete (PendingDelete[0]);
				PendingDelete.RemoveAt (0);
			}
		}

		public void DelayDelete(BaseEntity entity)
		{
			if (!PendingDelete.Contains(entity))
				PendingDelete.Add (entity);
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
			return Get(entity.GetType(), entity.Id) != null;
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

