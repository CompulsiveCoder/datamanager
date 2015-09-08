using System;
using datamanager.Data;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataManager
	{
		public DataIdManager IdManager;

		public EntitySaver Saver;
		public EntityDeleter Deleter;
		public EntityUpdater Updater;
		public EntityReader Reader;
		public EntityLister Lister;

		public DataManager ()
		{
			Construct ();
		}

		public void Construct()
		{
			IdManager = new DataIdManager ();
			Saver = new EntitySaver (IdManager);
			Deleter = new EntityDeleter (IdManager);
			Updater = new EntityUpdater (IdManager);
			Reader = new EntityReader (IdManager);
			Lister = new EntityLister (IdManager);
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
	}
}

