using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Feeds
{
	public class BookmakersFeed
	{
		public static string GetFeedUrl()
		{
			return "https://api-basketball.p.rapidapi.com/bookmakers";
		}

		public static BookmakersFeed FromJson(string json) => JsonConvert.DeserializeObject<BookmakersFeed>(json, Converter.Settings);

		[JsonProperty("get")]
		public string Get { get; set; }

		[JsonProperty("parameters")]
		public List<object> Parameters { get; set; }

		[JsonProperty("errors")]
		public List<object> Errors { get; set; }

		[JsonProperty("results")]
		public int Count { get; set; }

		[JsonProperty("response")]
		public List<Bookmaker> Bookmakers { get; set; }

		public class Bookmaker
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this BookmakersFeed self) => JsonConvert.SerializeObject(self, BasketballData.Processors.ApiBasketball.Feeds.Converter.Settings);
	}
}
