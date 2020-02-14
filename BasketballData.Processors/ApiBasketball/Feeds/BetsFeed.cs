using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Feeds
{
	public class BetsFeed
	{
		public static string GetFeedUrl()
		{
			return "https://api-basketball.p.rapidapi.com/bets";
		}

		public static BetsFeed FromJson(string json) => JsonConvert.DeserializeObject<BetsFeed>(json, Converter.Settings);

		[JsonProperty("get")]
		public string Get { get; set; }

		[JsonProperty("parameters")]
		public List<object> Parameters { get; set; }

		[JsonProperty("errors")]
		public List<object> Errors { get; set; }

		[JsonProperty("results")]
		public int Count { get; set; }

		[JsonProperty("response")]
		public List<BetType> BetTypes { get; set; }

		public class BetType
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this BetsFeed self) => JsonConvert.SerializeObject(self, BasketballData.Processors.ApiBasketball.Feeds.Converter.Settings);
	}
}

