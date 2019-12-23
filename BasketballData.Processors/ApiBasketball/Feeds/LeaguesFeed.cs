using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BasketballData.Processors.ApiBasketball.Feeds
{
	public class LeaguesFeed
	{
		public static string GetFeedUrl()
		{
			return "https://api-basketball.p.rapidapi.com/countries";
		}

		public static LeaguesFeed FromJson(string json) => JsonConvert.DeserializeObject<LeaguesFeed>(json, Converter.Settings);

		[JsonProperty("get")] 
		public string Endpoint { get; set; }

		[JsonProperty("parameters")]
		public List<object> Parameters { get; set; }

		[JsonProperty("errors")]
		public List<object> Errors { get; set; }

		[JsonProperty("results")]
		public long Results { get; set; }

		[JsonProperty("response")]
		public List<League> Leagues { get; set; }

		public class League
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("type")]
			public string Type { get; set; }

			[JsonProperty("logo")]
			public object Logo { get; set; }

			[JsonProperty("country")]
			public Country Country { get; set; }

			[JsonProperty("seasons")]
			public List<LeagueSeason> Seasons { get; set; }
		}

		public partial class Country
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("code")]
			public string Code { get; set; }

			[JsonProperty("flag")]
			public Uri Flag { get; set; }
		}

		public partial class LeagueSeason
		{
			[JsonProperty("season")]
			public string Season { get; set; }

			[JsonProperty("start")]
			public DateTimeOffset Start { get; set; }

			[JsonProperty("end")]
			public DateTimeOffset End { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this LeaguesFeed self) => JsonConvert.SerializeObject(self, BasketballData.Processors.ApiBasketball.Feeds.Converter.Settings);
	}
}
