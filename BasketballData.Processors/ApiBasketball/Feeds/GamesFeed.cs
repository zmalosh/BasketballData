using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Feeds
{
	public class GamesFeed
	{
		public static string GetFeedUrl(int apiLeagueId, string apiSeasonKey)
		{
			return $"https://api-basketball.p.rapidapi.com/games?league={apiLeagueId}&season={apiSeasonKey}";
		}

		public static GamesFeed FromJson(string json) => string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<GamesFeed>(json, Converter.Settings);

		[JsonProperty("get")]
		public string Get { get; set; }

		[JsonProperty("parameters")]
		public GameParameters Parameters { get; set; }

		[JsonProperty("errors")]
		public List<object> Errors { get; set; }

		[JsonProperty("results")]
		public int Count { get; set; }

		[JsonProperty("response")]
		public List<ApiGame> Games { get; set; }

		public class GameParameters
		{
			[JsonProperty("league")]
			public int? League { get; set; }

			[JsonProperty("season")]
			public string Season { get; set; }

			[JsonProperty("id")]
			public int? Id { get; set; }

			[JsonProperty("team")]
			public int? Team { get; set; }

			[JsonProperty("date")]
			public string Date { get; set; }

			[JsonProperty("timezone")]
			public string Timezone { get; set; }
		}

		public class ApiGame
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("date")]
			public DateTimeOffset Date { get; set; }

			[JsonProperty("time")]
			public string Time { get; set; }

			[JsonProperty("timestamp")]
			public int Timestamp { get; set; }

			[JsonProperty("timezone")]
			public string Timezone { get; set; }

			[JsonProperty("stage")]
			public string Stage { get; set; }

			[JsonProperty("week")]
			public string Week { get; set; }

			[JsonProperty("status")]
			public GameStatus Status { get; set; }

			[JsonProperty("league")]
			public League League { get; set; }

			[JsonProperty("country")]
			public Country Country { get; set; }

			[JsonProperty("teams")]
			public Teams Teams { get; set; }

			[JsonProperty("scores")]
			public GameScore Scores { get; set; }
		}

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

		public class League
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("type")]
			public string Type { get; set; }

			[JsonProperty("season")]
			public string Season { get; set; }

			[JsonProperty("logo")]
			public string Logo { get; set; }
		}

		public class GameScore
		{
			[JsonProperty("home")]
			public LineScore Home { get; set; }

			[JsonProperty("away")]
			public LineScore Away { get; set; }
		}

		public class LineScore
		{
			[JsonProperty("quarter_1")]
			public int? Quarter1 { get; set; }

			[JsonProperty("quarter_2")]
			public int? Quarter2 { get; set; }

			[JsonProperty("quarter_3")]
			public int? Quarter3 { get; set; }

			[JsonProperty("quarter_4")]
			public int? Quarter4 { get; set; }

			[JsonProperty("over_time")]
			public int? OverTime { get; set; }

			[JsonProperty("total")]
			public int? Total { get; set; }
		}

		public class GameStatus
		{
			[JsonProperty("long")]
			public string Long { get; set; }

			[JsonProperty("short")]
			public string Short { get; set; }

			[JsonProperty("timer")]
			public int? Timer { get; set; }
		}

		public class Teams
		{
			[JsonProperty("home")]
			public ApiTeam Home { get; set; }

			[JsonProperty("away")]
			public ApiTeam Away { get; set; }
		}

		public class ApiTeam
		{
			[JsonProperty("id")]
			public int? Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("logo")]
			public string Logo { get; set; }

			public class ApiTeamComparer : IEqualityComparer<ApiTeam>
			{
				public bool Equals([AllowNull] ApiTeam x, [AllowNull] ApiTeam y)
				{
					if (x == null || y == null || !x.Id.HasValue || !y.Id.HasValue) { return false; }
					return x.Id.Value == y.Id.Value && ((string.IsNullOrEmpty(x.Name) && string.IsNullOrEmpty(y.Name)) || (x.Name == y.Name));
				}

				public int GetHashCode([DisallowNull] ApiTeam obj)
				{
					return base.GetHashCode();
				}
			}
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this GamesFeed self) => JsonConvert.SerializeObject(self, BasketballData.Processors.ApiBasketball.Feeds.Converter.Settings);
	}
}
