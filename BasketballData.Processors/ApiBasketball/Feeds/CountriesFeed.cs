using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BasketballData.Processors.ApiBasketball.Feeds
{
	public class CountriesFeed
	{
		public static string GetFeedUrl()
		{
			return "https://api-basketball.p.rapidapi.com/countries";
		}

		public static CountriesFeed FromJson(string json) => JsonConvert.DeserializeObject<CountriesFeed>(json, Converter.Settings);

		[JsonProperty("get")]
		public string Endpoint { get; set; }

		[JsonProperty("parameters")]
		public List<object> Parameters { get; set; }

		[JsonProperty("errors")]
		public List<object> Errors { get; set; }

		[JsonProperty("results")]
		public int ResultCount { get; set; }

		[JsonProperty("response")]
		public List<Country> Countries { get; set; }

		public class Country
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("code")]
			public string Code { get; set; }

			[JsonProperty("flag")]
			public string Flag { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this CountriesFeed self) => JsonConvert.SerializeObject(self, BasketballData.Processors.ApiBasketball.Feeds.Converter.Settings);
	}
}
