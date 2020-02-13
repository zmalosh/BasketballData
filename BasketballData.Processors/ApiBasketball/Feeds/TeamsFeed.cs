using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Feeds
{
	public class TeamsFeed
	{
		public static string GetFeedUrl(int apiLeagueId, string apiSeasonKey)
		{
			return $"https://api-basketball.p.rapidapi.com/teams?league={apiLeagueId}&season={apiSeasonKey}";
		}

		public static TeamsFeed FromJson(string json) => JsonConvert.DeserializeObject<TeamsFeed>(json, Converter.Settings);

		[JsonProperty("get")]
		public string Get { get; set; }

		[JsonProperty("parameters")]
		public TeamParameters Parameters { get; set; }

		[JsonProperty("errors")]
		public List<object> Errors { get; set; }

		[JsonProperty("results")]
		public int Count { get; set; }

		[JsonProperty("response")]
		public List<ApiFullTeam> Teams { get; set; }

		public class TeamParameters
		{
			[JsonProperty("league")]
			public int? League { get; set; }

			[JsonProperty("season")]
			public string Season { get; set; }

			[JsonProperty("id")]
			public int? Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("search")]
			public string Search { get; set; }
		}

		public class ApiFullTeam
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("nationnal")]
			public bool IsNational { get; set; }

			[JsonProperty("logo")]
			public string Logo { get; set; }

			[JsonProperty("country")]
			public ApiCountry Country { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this TeamsFeed self) => JsonConvert.SerializeObject(self, BasketballData.Processors.ApiBasketball.Feeds.Converter.Settings);
	}
}
