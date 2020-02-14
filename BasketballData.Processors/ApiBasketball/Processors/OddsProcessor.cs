using BasketballData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Processors
{
	public class OddsProcessor : IProcessor
	{
		private readonly int apiBasketballLeagueId;
		private readonly string apiBasketballSeasonKey;
		private readonly Dictionary<int, int> betTypesDict;
		private readonly Dictionary<int, int> bookmakersDict;

		public OddsProcessor(int apiBasketballLeagueId, string apiBasketballSeasonKey,
			Dictionary<int, int> betTypesDict,
			Dictionary<int, int> bookmakersDict)
		{
			this.apiBasketballLeagueId = apiBasketballLeagueId;
			this.apiBasketballSeasonKey = apiBasketballSeasonKey;
			this.betTypesDict = betTypesDict;
			this.bookmakersDict = bookmakersDict;
		}

		public void Run(BasketballDataContext dbContext)
		{
			var url = Feeds.OddsFeed.GetFeedUrl(this.apiBasketballLeagueId, this.apiBasketballSeasonKey);
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			if (!string.IsNullOrEmpty(rawJson))
			{
				var feed = Feeds.OddsFeed.FromJson(rawJson);

				int leagueSeasonId = dbContext.LeagueSeasons.First(x => x.ApiBasketballLeagueId == this.apiBasketballLeagueId && x.ApiBasketballSeasonKey == this.apiBasketballSeasonKey).LeagueSeasonId;
				var gameDict = dbContext.Games.Where(x => x.LeagueSeasonId == leagueSeasonId).ToDictionary(x => x.ApiBasketballGameId, y => y.GameId);

				var apiOddsGames = feed.OddsGames.ToList();
				foreach (var apiOddsGame in apiOddsGames)
				{
					int gameId = gameDict[apiOddsGame.Game.Id];

					var allGameLines = dbContext.BetLines.Where(x => x.GameId == gameId).ToList();
					foreach (var apiBookmaker in apiOddsGame.Bookmakers)
					{
						int bookmakerId = this.bookmakersDict[apiBookmaker.BookmakerId];
						foreach (var apiBetType in apiBookmaker.BetTypes)
						{
							int betTypeId = this.betTypesDict[apiBetType.BetTypeId];
							foreach (var apiBetLine in apiBetType.BetLines)
							{
								decimal? betValue = null;
								string betName = apiBetLine.BetName;
								if (betName.StartsWith("Over ", StringComparison.InvariantCultureIgnoreCase)
									|| betName.StartsWith("Under ", StringComparison.InvariantCultureIgnoreCase))
								{
									string[] arrBetName = betName.Split(' ');
									betName = arrBetName[0].ToUpper();
									if (arrBetName.Length > 1)
									{
										string strBetValue = arrBetName[1];
										if (!string.IsNullOrEmpty(strBetValue))
										{
											betValue = decimal.Parse(strBetValue);
										}
									}
								}
								var dbBetLine = allGameLines.SingleOrDefault(x => x.GameId == gameId && x.BookmakerId == bookmakerId && x.BetTypeId == betTypeId && x.BetName == betName);
								if (dbBetLine == null)
								{
									dbBetLine = new BetLine
									{
										GameId = gameId,
										BookmakerId = bookmakerId,
										BetTypeId = betTypeId,
										BetName = betName,
										Line = apiBetLine.Line_Decimal
									};
									allGameLines.Add(dbBetLine);
									dbContext.BetLines.Add(dbBetLine);
								}
							}
						}
					}
				}
			}
		}
	}
}
