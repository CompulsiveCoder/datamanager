using System;
using NUnit.Framework;
using datamanager.Entities;
using datamanager.Data.Providers.Memory;

namespace datamanager.Data.Tests.Unit
{
    [TestFixture(Category="Unit")]
	public class DataTypeManagerUnitTestFixture : BaseDataTestFixture
	{
		[Test]
		public void Test_Add()
		{
            var data = GetDataManager ();

            var typeManager = new DataTypeManager (data.Settings, data.Provider);

			var exampleType = typeof(ExampleArticle);

			typeManager.Add (exampleType);

            var memoryProvider = (MemoryDataProvider)data.Provider;

            var internalData = memoryProvider.Data;

            var key = data.Settings.Prefix + "-" + typeManager.TypesKey;

            var typesString = internalData [key];

            var expectedTypesString = exampleType.Name + typeManager.TypeNamesParser.PairSeparator + exampleType.AssemblyQualifiedName;

			Assert.AreEqual (expectedTypesString, typesString);
		}

		[Test]
		public void Test_GetType()
        {
            Console.WriteLine ("====================");
            Console.WriteLine ("Preparing test");
            Console.WriteLine ("====================");

            var data = GetDataManager ();

            var typeManager = new DataTypeManager (data.Settings, data.Provider);

			var exampleType = typeof(ExampleArticle);

			var typesString = exampleType.Name + typeManager.TypeNamesParser.PairSeparator + exampleType.AssemblyQualifiedName;

            Console.WriteLine ("Types string: " + typesString);

            var key = data.Settings.Prefix + "-" + typeManager.TypesKey;

            Console.WriteLine ("Types data key: " + key);

            var memoryProvider = (MemoryDataProvider)data.Provider;

            var internalData = memoryProvider.Data;

            internalData [key] = typesString;

            Console.WriteLine ("====================");
            Console.WriteLine ("Executing test");
            Console.WriteLine ("====================");

			var type = typeManager.GetType (exampleType.Name);

            Console.WriteLine ("====================");
            Console.WriteLine ("Reviewing test");
            Console.WriteLine ("====================");

			Assert.IsNotNull (type);
			Assert.AreEqual (exampleType, type);
		}


		[Test]
		public void Test_GetTypes()
		{
            var data = GetDataManager ();

			var typeManager = new DataTypeManager (data.Settings, data.Provider);

			var exampleType = typeof(ExampleArticle);

            var typesString = exampleType.Name + typeManager.TypeNamesParser.PairSeparator + exampleType.AssemblyQualifiedName;

			var key = data.Settings.Prefix + "-Types";

            var memoryProvider = (MemoryDataProvider)data.Provider;

            var internalData = memoryProvider.Data;

            internalData [key] = typesString;

			var types = typeManager.GetTypes();

			Assert.AreEqual (1, types.Count);

			Assert.IsTrue (types.ContainsKey (exampleType.Name));

			var fullTypeName = types [exampleType.Name];

			Assert.AreEqual (exampleType.AssemblyQualifiedName, fullTypeName);
		}
	}
}

