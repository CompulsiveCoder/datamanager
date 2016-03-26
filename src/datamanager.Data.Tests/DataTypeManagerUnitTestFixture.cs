using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests
{
	[TestFixture]
	public class DataTypeManagerUnitTestFixture : BaseDataTestFixture
	{
		[Test]
		public void Test_Add()
		{
			var context = GetTestContext ();

			var typeManager = new DataTypeManager (context.Keys, context.DataClient);

			var exampleType = typeof(ExampleArticle);

			typeManager.Add (exampleType);

			var data = context.DataClient.Data;

			var key = context.Settings.Prefix + "-Types";

			var typesString = data [key];

			var expectedTypesString = exampleType.Name + typeManager.PairSeparator + exampleType.AssemblyQualifiedName;

			Assert.AreEqual (expectedTypesString, typesString);
		}

		[Test]
		public void Test_GetType()
		{
			var context = GetTestContext ();

			var typeManager = new DataTypeManager (context.Keys, context.DataClient);

			var exampleType = typeof(ExampleArticle);

			var typesString = exampleType.Name + typeManager.PairSeparator + exampleType.AssemblyQualifiedName;

			var key = context.Settings.Prefix + "-Types";

			context.DataClient.Data [key] = typesString;

			var type = typeManager.GetType (exampleType.Name);

			Assert.IsNotNull (type);
			Assert.AreEqual (exampleType, type);
		}


		[Test]
		public void Test_GetTypes()
		{
			var context = GetTestContext ();

			var typeManager = new DataTypeManager (context.Keys, context.DataClient);

			var exampleType = typeof(ExampleArticle);

			var typesString = exampleType.Name + typeManager.PairSeparator + exampleType.AssemblyQualifiedName;

			var key = context.Settings.Prefix + "-Types";

			context.DataClient.Data [key] = typesString;

			var types = typeManager.GetTypes();

			Assert.AreEqual (1, types.Count);

			Assert.IsTrue (types.ContainsKey (exampleType.Name));

			var fullTypeName = types [exampleType.Name];

			Assert.AreEqual (exampleType.AssemblyQualifiedName, fullTypeName);
		}
	}
}

