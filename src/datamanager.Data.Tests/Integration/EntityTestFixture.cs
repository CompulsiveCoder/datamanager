using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Integration
{
	[TestFixture]
	public class EntityTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SaveAndGet()
		{
			var saver = new EntitySaver ();
			var entity = new ExampleEntity ("TestEntity", 10);
			saver.Save (entity);

			var reader = new EntityReader ();
			var loadedEntity = reader.Read<ExampleEntity>(entity.Id);

			Assert.IsNotNull (loadedEntity);

			var indexer = new EntityLister ();
			var loadedEntities = indexer.Get<ExampleEntity>();

			Assert.IsNotNull (loadedEntities);
			Assert.AreEqual (1, loadedEntities.Length);

			var deleter = new EntityDeleter ();
			deleter.Delete (entity);
		}
	}
}

