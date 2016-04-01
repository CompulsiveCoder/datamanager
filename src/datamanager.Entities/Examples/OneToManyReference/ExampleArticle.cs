using System;
using Newtonsoft.Json;

namespace datamanager.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleArticle : BaseEntity
	{
		[TwoWay("Articles")]
		public ExampleAuthor Author { get; set; }

		public ExampleArticle ()
		{
		}
	}
}

