﻿using System;
using Sider;
using System.Collections.Generic;
using datamanager.Entities;

namespace datamanager.Data
{
	public class DataLister : BaseDataAdapter
	{
		public DataTypeManager TypeManager;
		public DataIdManager IdManager;
		public DataReader Reader;

		public DataLister (DataTypeManager typeManager, DataIdManager idManager, DataReader reader, BaseRedisClientWrapper client) : base (client)
		{
			TypeManager = typeManager;
			IdManager = idManager;
			Reader = reader;
		}


		public T[] Get<T>()
		{
			var ids = IdManager.GetIds(typeof(T).Name);

			var entities = new List<T> ();
			foreach (string id in ids) {
				entities.Add (Reader.Read<T>(id));
			}
			return entities.ToArray();
		}

		public BaseEntity[] Get(string entityTypeName)
		{
			var ids = IdManager.GetIds(entityTypeName);

			var entities = new List<BaseEntity> ();
			foreach (string id in ids) {
				entities.Add (Reader.Read(entityTypeName, id));
			}
			return entities.ToArray();
		}

		public BaseEntity[] GetAll()
		{
			var entities = new List<BaseEntity> ();

			var types = TypeManager.GetTypeNames ();

			foreach (var typeName in types) {
				var ids = IdManager.GetIds (typeName);

				foreach (string id in ids) {
					entities.Add (Reader.Read (typeName, id));
				}
			}
			return entities.ToArray();
		}
	}
}

