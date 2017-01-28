using System;
using NUnit.Framework;
using datamanager.Entities;
using datamanager.Data.Providers.Memory;

namespace datamanager.Data.Tests.Unit
{
    [TestFixture]
    public class DataManagerPrefixBypasserUnitTestFixture : BaseDataUnitTestFixture
    {
        [Test]
        public void Test_FindKeysFromType()
        {
            Console.WriteLine ("====================");
            Console.WriteLine ("Preparing test");
            Console.WriteLine ("====================");

            var data = GetDataManager ();

            var bypasser = new DataManagerPrefixBypasser (data.Settings, data.Provider);

            var provider = (MemoryDataProvider)data.Provider;

            var internalData = provider.Data;

            internalData.Add ("Prefix-12345-" + typeof(ExampleArticle).Name + "-12345", "{Title=\"Test\"}");
            internalData.Add ("Prefix-67890-" + typeof(ExampleArticle).Name + "-67890", "{Title=\"Test\"}");

            Console.WriteLine ("");
            Console.WriteLine ("====================");
            Console.WriteLine ("Executing test");
            Console.WriteLine ("====================");

            var keys = bypasser.FindKeysFromType (typeof(ExampleArticle).Name);

            Console.WriteLine ("");
            Console.WriteLine ("====================");
            Console.WriteLine ("Reviewing test");
            Console.WriteLine ("====================");

            Assert.AreEqual (2, keys.Length);
        }

    }
}

