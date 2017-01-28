using System;
using NUnit.Framework;
using datamanager.Entities;
using System.IO;
using datamanager.Data.Tests;
using datamanager.Data.Providers.Memory;

namespace datamanager.Data.Tests.Unit
{
    [TestFixture(Category="Unit")]
    public class DataListerUnitTestFixture : BaseDataUnitTestFixture
    {
        [Test]
        public void Test_List()
        {
            Console.WriteLine ("====================");
            Console.WriteLine ("Preparing test");
            Console.WriteLine ("====================");

            var data = GetDataManager ();

            var internalData = ((MemoryDataProvider)data.Provider).Data;

            var exampleArticle = new ExampleArticle ();

            var json = exampleArticle.ToJson ();

            var entityKey = data.Settings.Prefix + "-" + exampleArticle.TypeName + "-" + exampleArticle.Id;

            Console.WriteLine ("  Entity key: " + entityKey);

            internalData[entityKey] = json;

            var idsKey = data.Settings.Prefix + "-" + exampleArticle.TypeName + ":Ids";

            Console.WriteLine ("  IDs key: " + idsKey);

            var idsString = exampleArticle.Id;

            internalData[idsKey] = idsString;

            Console.WriteLine ("  IDs string: " + idsString);

            var typesString = exampleArticle.TypeName + ":" + exampleArticle.GetType ().Name + ", " + exampleArticle.GetType ().Assembly.GetName ().Name;

            Console.WriteLine ("  Types string: " + typesString);

            var typesKey = data.Settings.Prefix + "-" + data.TypeManager.TypesKey;

            Console.WriteLine ("  Types key: " + typesKey);

            internalData[typesKey] = typesString;

            var lister = new DataLister (data.Settings, data.TypeManager, data.IdManager, data.Reader, data.Provider);

            Console.WriteLine ("");
            Console.WriteLine ("====================");
            Console.WriteLine ("Executing test");
            Console.WriteLine ("====================");

            var articles = lister.Get<ExampleArticle> ();

            Console.WriteLine ("");
            Console.WriteLine ("====================");
            Console.WriteLine ("Reviewing test");
            Console.WriteLine ("====================");

            Assert.AreEqual (1, articles.Length);

            Assert.IsNotNull (articles [0]);

            Assert.AreEqual (exampleArticle.Id, articles [0].Id);
        }
    }
}

