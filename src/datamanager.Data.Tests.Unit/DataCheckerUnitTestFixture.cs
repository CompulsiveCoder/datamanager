using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Unit
{
    [TestFixture(Category="Unit")]
    public class DataCheckerUnitTestFixture : BaseDataUnitTestFixture
	{
		[Test]
		public void Test_Check()
		{
            var data = GetDataManager ();

			var checker = new DataChecker (data.Reader, data.Settings);

			var exampleArticle = new ExampleArticle ();

			data.Saver.Save (exampleArticle);

			// TODO: Make this test fully isolated instead of using the Save function above

			// TODO: clean up
			//var key = Context.Settings.Prefix + "-" + exampleArticle.GetType ().FullName + "-" + exampleArticle.Id;

			//Context.DataClient.Data [key] = exampleArticle.ToJson ();

			var exists = checker.Exists (exampleArticle);

			Assert.IsTrue (exists);
		}
	}
}

