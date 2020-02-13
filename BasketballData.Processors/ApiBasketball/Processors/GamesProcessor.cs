using BasketballData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Processors
{
	public class GamesProcessor : IProcessor
	{
		private readonly int apiBasketballLeagueId;
		private readonly string apiBasketballSeasonKey;
		private readonly Dictionary<int, int> countriesDict;
		private readonly Dictionary<int, int> teamsDict;
		private readonly Dictionary<string, FullGameStatus> statusDict;

		public GamesProcessor(int apiBasketballLeagueId, string apiBasketballSeasonKey,
			Dictionary<int, int> countriesDict,
			Dictionary<int, int> teamsDict,
			Dictionary<string, FullGameStatus> statusDict)
		{
			this.apiBasketballLeagueId = apiBasketballLeagueId;
			this.apiBasketballSeasonKey = apiBasketballSeasonKey;
			this.countriesDict = countriesDict;
			this.teamsDict = teamsDict;
			this.statusDict = statusDict;
		}

		public void Run(BasketballDataContext dbContext)
		{
			var url = Feeds.GamesFeed.GetFeedUrl(this.apiBasketballLeagueId, this.apiBasketballSeasonKey);
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.GamesFeed.FromJson(rawJson);

			int leagueSeasonId = dbContext.LeagueSeasons.First(x => x.ApiBasketballLeagueId == this.apiBasketballLeagueId && x.ApiBasketballSeasonKey == this.apiBasketballSeasonKey).LeagueSeasonId;

			var existingGames = dbContext.Games.ToDictionary(x => x.ApiBasketballGameId, y => y);
			var apiGames = feed.Games.OrderBy(x => x.Date).ThenBy(y => y.Id).ToList();

			foreach (var apiGame in apiGames)
			{
				if (apiGame?.Teams?.Home?.Id != null && apiGame.Teams.Away?.Id != null)
				{
					FullGameStatus apiStatus = string.IsNullOrEmpty(apiGame.Status?.Short) ? FullGameStatus.Unknown : statusDict[apiGame.Status.Short];
					if (!existingGames.TryGetValue(apiGame.Id, out Game dbGame))
					{
						dbGame = new Game
						{
							ApiBasketballGameId = apiGame.Id,
							AwayTeamId = this.teamsDict[apiGame.Teams.Away.Id.Value],
							CountryId = this.countriesDict[apiGame.Country.Id],
							FullGameStatusId = apiStatus,
							GameTimeUtc = apiGame.Date.UtcDateTime,
							HomeTeamId = this.teamsDict[apiGame.Teams.Home.Id.Value],
							LeagueSeasonId = leagueSeasonId,
							QtrTimeRem = apiGame.Status?.Timer
						};

						if (apiGame.Scores != null)
						{
							SetScoreValuesInDb(apiGame, dbGame);
						}

						existingGames.Add(apiGame.Id, dbGame);
						dbContext.Games.Add(dbGame);
					}
					else if (IsApiUpdated(apiGame, dbGame, apiStatus))
					{
						dbGame.AwayTeamId = teamsDict[apiGame.Teams.Away.Id.Value];
						dbGame.FullGameStatusId = apiStatus;
						dbGame.GameTimeUtc = apiGame.Date.UtcDateTime;
						dbGame.HomeTeamId = teamsDict[apiGame.Teams.Home.Id.Value];
						dbGame.QtrTimeRem = apiGame.Status?.Timer;

						if (apiGame.Scores != null)
						{
							SetScoreValuesInDb(apiGame, dbGame);
						}
					}
				}
			}
		}


		private bool IsApiUpdated(Feeds.GamesFeed.ApiGame apiGame, Game dbGame, FullGameStatus apiStatus)
		{
			return dbGame.HomeScore != apiGame.Scores.Home.Total
			|| dbGame.AwayScore != apiGame.Scores.Away.Total
			|| (apiStatus != FullGameStatus.Unknown && dbGame.FullGameStatusId != apiStatus) // DON'T OVERRIDE WITH UNKNOWN
			|| dbGame.HomeScore_Q1 != apiGame.Scores.Home.Quarter1
			|| dbGame.HomeScore_Q2 != apiGame.Scores.Home.Quarter2
			|| dbGame.HomeScore_Q3 != apiGame.Scores.Home.Quarter3
			|| dbGame.HomeScore_Q4 != apiGame.Scores.Home.Quarter4
			|| dbGame.HomeScore_OT != apiGame.Scores.Home.OverTime
			|| dbGame.GameTimeUtc != apiGame.Date.UtcDateTime
			|| dbGame.AwayScore_Q1 != apiGame.Scores.Away.Quarter1
			|| dbGame.AwayScore_Q2 != apiGame.Scores.Away.Quarter2
			|| dbGame.AwayScore_Q3 != apiGame.Scores.Away.Quarter3
			|| dbGame.AwayScore_Q4 != apiGame.Scores.Away.Quarter4
			|| dbGame.AwayScore_OT != apiGame.Scores.Away.OverTime;
		}

		private void SetScoreValuesInDb(Feeds.GamesFeed.ApiGame apiGame, Game dbGame)
		{
			dbGame.HomeScore = apiGame.Scores.Home.Total;
			dbGame.HomeScore_Q1 = apiGame.Scores.Home.Quarter1;
			dbGame.HomeScore_Q2 = apiGame.Scores.Home.Quarter2;
			dbGame.HomeScore_Q3 = apiGame.Scores.Home.Quarter3;
			dbGame.HomeScore_Q4 = apiGame.Scores.Home.Quarter4;
			dbGame.HomeScore_OT = apiGame.Scores.Home.OverTime;
			dbGame.AwayScore = apiGame.Scores.Away.Total;
			dbGame.AwayScore_Q1 = apiGame.Scores.Away.Quarter1;
			dbGame.AwayScore_Q2 = apiGame.Scores.Away.Quarter2;
			dbGame.AwayScore_Q3 = apiGame.Scores.Away.Quarter3;
			dbGame.AwayScore_Q4 = apiGame.Scores.Away.Quarter4;
			dbGame.AwayScore_OT = apiGame.Scores.Away.OverTime;
		}
	}
}
