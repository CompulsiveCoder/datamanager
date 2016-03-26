using System;
using Sider;
using datamanager.Entities;
using System.Collections.Generic;

namespace datamanager.Data
{
	public class DataUpdater : BaseDataAdapter
	{
		public DataManagerSettings Settings;

		public DataKeys Keys;

		public DataChecker Checker;
		public DataLinker Linker;
		public DataPreparer Preparer;

		public List<BaseEntity> PendingUpdate = new List<BaseEntity>();

		public DataUpdater (DataManagerSettings settings, DataKeys keys, DataLinker linker, DataPreparer preparer, DataChecker checker, BaseRedisClientWrapper client) : base(client)
		{
			Settings = settings;
			Keys = keys;
			Linker = linker;
			Preparer = preparer;
			Checker = checker;
		}

		public void Update(BaseEntity entity)
		{
			if (Checker.Exists (entity)) {
				if (Settings.IsVerbose)
					Console.WriteLine ("Updating: " + entity.GetType ().Name);

				Linker.CommitLinks (entity);

				InternalUpdate (entity);
			} else// if (!Data.PendingSave.Contains(entity)) // TODO: Remove if not needed
				throw new EntityNotFoundException (entity);
		}

		public void InternalUpdate(BaseEntity entity)
		{
			var key = Keys.GetKey (entity);
			Client.Set (key, Preparer.PrepareForStorage (entity).ToJson ());
		}

		public void DelayUpdate(BaseEntity entity)
		{
			if (!PendingUpdate.Contains(entity))
				PendingUpdate.Add (entity);
		}

		public void CommitPendingUpdates()
		{
			while (PendingUpdate.Count > 0)
			{
				try
				{
					var entity = PendingUpdate[0];
					if (Checker.Exists(entity))
					{
						Update (entity);
						PendingUpdate.RemoveAt (0);
					}
				}
				catch (EntityNotFoundException ex) {
					// TODO: Check if this exception should be thrown
					throw new UnsavedLinksException (ex.Entity);
				}
			}
		}
	}
}

