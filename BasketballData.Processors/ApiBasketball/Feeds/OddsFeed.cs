using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Feeds
{
	public class OddsFeed
	{
		public static string GetFeedUrl(int apiBasketballLeagueId, string apiBasketballSeasonKey)
		{
			return $"https://api-basketball.p.rapidapi.com/odds?league={apiBasketballLeagueId}&season={apiBasketballSeasonKey}";
		}

		public static OddsFeed FromJson(string json) => JsonConvert.DeserializeObject<OddsFeed>(json, Converter.Settings);

		[JsonProperty("get")]
		public string Get { get; set; }

		[JsonProperty("parameters")]
		public OddsParameters Parameters { get; set; }

		[JsonProperty("errors")]
		public List<object> Errors { get; set; }

		[JsonProperty("results")]
		public int Count { get; set; }

		[JsonProperty("response")]
		public List<OddsGame> OddsGames { get; set; }

		public class OddsParameters
		{
			[JsonProperty("league")]
			public int? League { get; set; }

			[JsonProperty("season")]
			public string Season { get; set; }

			[JsonProperty("game")]
			public int? Game { get; set; }

			[JsonProperty("bookmaker")]
			public int? Bookmaker { get; set; }

			[JsonProperty("bet")]
			public int? Bet { get; set; }
		}

		public class OddsGame
		{
			[JsonProperty("league")]
			public LeagueInfo League { get; set; }

			[JsonProperty("country")]
			public ApiCountry Country { get; set; }

			[JsonProperty("game")]
			public GameInfo Game { get; set; }

			[JsonProperty("bookmakers")]
			public List<Bookmaker> Bookmakers { get; set; }
		}

		public class Bookmaker
		{
			[JsonProperty("id")]
			public int BookmakerId { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("bets")]
			public List<BetType> BetTypes { get; set; }
		}

		public class BetType
		{
			[JsonProperty("id")]
			public int BetTypeId { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("values")]
			public List<BetLine> BetLines { get; set; }
		}

		public class BetLine
		{
			[JsonProperty("value")]
			public string BetValue { get; set; }

			[JsonProperty("odd")]
			public decimal Line_Decimal { get; set; }
		}

		public class GameInfo
		{
			[JsonProperty("id")]
			public int Id { get; set; }
		}

		public class LeagueInfo
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
	}

	public static partial class Serialize
	{
		public static string ToJson(this OddsFeed self) => JsonConvert.SerializeObject(self, BasketballData.Processors.ApiBasketball.Feeds.Converter.Settings);
	}
}
