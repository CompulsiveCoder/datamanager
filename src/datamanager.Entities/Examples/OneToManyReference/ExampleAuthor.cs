using System;
using Newtonsoft.Json;

namespace datamanager.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleAuthor : BaseEntity
	{
		[TwoWay("Author")]
		public ExampleArticle[] Articles { get;set; }
	}
}

