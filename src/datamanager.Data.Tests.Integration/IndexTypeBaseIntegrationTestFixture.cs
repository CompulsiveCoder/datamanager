using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Integration
{
    [TestFixture(Category="Integration")]
    public class IndexTypeBaseIntegrationTestFixture : BaseDataIntegrationTestFixture
    {
        [Test]
        public void Test_SaveAndGetIndexType()
        {   
            var entity = new DerivedEntity ();

            var data = GetDataManager ();

            data.Save (entity);

            var types = data.TypeManager.GetTypes ();

            Assert.AreEqual (2, types.Count);
            Assert.IsTrue (types.ContainsKey ("DerivedEntity"));
            Assert.IsTrue (types.ContainsKey ("InheritedEntity"));

            var derivedEntityIds = data.IdManager.GetIds ("InheritedEntity");

            // Now the Get function should work when specifying the index type
            var foundEntities = data.Get<InheritedEntity> ();

            Assert.AreEqual (1, foundEntities.Length);

            // TODO: These asserts might be better off in unit tests rather than an integration test
            Assert.AreEqual (1, derivedEntityIds.Length);

            Assert.AreEqual (entity.Id, derivedEntityIds [0]);

            var inheritedEntityIds = data.IdManager.GetIds ("InheritedEntity");

            Assert.AreEqual (1, inheritedEntityIds.Length);

            Assert.AreEqual (entity.Id, inheritedEntityIds [0]);
        }
    }
}

