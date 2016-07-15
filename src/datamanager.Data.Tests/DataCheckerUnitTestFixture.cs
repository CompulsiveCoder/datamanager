using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests
{
    [TestFixture(Category="Unit")]
	public class DataCheckerUnitTestFixture : BaseDataTestFixture
	{
		[Test]
		public void Test_Check()
		{
			var context = GetTestContext ();

			var checker = new DataChecker (context.Reader, context.Settings);

			var exampleArticle = new ExampleArticle ();

			context.Saver.Save (exampleArticle);

			// TODO: Make this test fully isolated instead of using the Save function above

			// TODO: clean up
			//var key = Context.Settings.Prefix + "-" + exampleArticle.GetType ().FullName + "-" + exampleArticle.Id;

			//Context.DataClient.Data [key] = exampleArticle.ToJson ();

			var exists = checker.Exists (exampleArticle);

			Assert.IsTrue (exists);
		}
	}
}

