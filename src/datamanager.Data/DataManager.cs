﻿using System;
using datamanager.Data;
using datamanager.Entities;
using Sider;

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

		public void Save(BaseEntity entity)
		{
			Saver.Save (entity);
		}

		public void Update(BaseEntity entity)
		{
			Updater.Update (entity);
		}

		public void Delete(BaseEntity entity)
		{
			Deleter.Delete (entity);
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

		#region IDisposable implementation

		public void Dispose ()
		{
			Client.Quit ();
		}

		#endregion
	}
}

