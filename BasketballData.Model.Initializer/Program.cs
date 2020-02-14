using BasketballData.Processors.ApiBasketball.Processors;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BasketballData.Model.Initializer
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = GetConfig();

			Console.WriteLine("Hello World!");
			using (var context = new BasketballDataContext())
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();
				context.SaveChanges();

				context.Countries.Add(
					new Country
					{
						ApiBasketballCountryId = 0,
						CountryAbbr = "XX",
						CountryName = "World",
						FlagUrl = null
					});

				context.RefGameStatuses.AddRange(new List<RefGameStatus>
				{
					new RefGameStatus{ FullGameStatusId = FullGameStatus.NotStarted, FullGameStatusName = "Not Started", GameStatusId = GameStatus.Pregame, GameStatusName = "Pregame", ApiBasketballStatusCode = "NS" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Live_Q1, FullGameStatusName = "Live - Q1", GameStatusId = GameStatus.Live, GameStatusName = "Live", ApiBasketballStatusCode = "Q1" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Live_Q2, FullGameStatusName = "Live - Q2", GameStatusId = GameStatus.Live, GameStatusName = "Live", ApiBasketballStatusCode = "Q2" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Live_Q3, FullGameStatusName = "Live - Q3", GameStatusId = GameStatus.Live, GameStatusName = "Live", ApiBasketballStatusCode = "Q3" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Live_Q4, FullGameStatusName = "Live - Q4", GameStatusId = GameStatus.Live, GameStatusName = "Live", ApiBasketballStatusCode = "Q4" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Live_OT, FullGameStatusName = "Live - OT", GameStatusId = GameStatus.Live, GameStatusName = "Live", ApiBasketballStatusCode = "OT" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Live_BK, FullGameStatusName = "Live - Break", GameStatusId = GameStatus.Live, GameStatusName = "Live", ApiBasketballStatusCode = "BT" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Live_HT, FullGameStatusName = "Live - Halftime", GameStatusId = GameStatus.Live, GameStatusName = "Live", ApiBasketballStatusCode = "HT" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Final, FullGameStatusName = "Final", GameStatusId = GameStatus.Final, GameStatusName = "Final", ApiBasketballStatusCode = "FT" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.FinalWithOT, FullGameStatusName = "Final (OT)", GameStatusId = GameStatus.Final, GameStatusName = "Final", ApiBasketballStatusCode = "AOT" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Postponed, FullGameStatusName = "Postponed", GameStatusId = GameStatus.Postponed, GameStatusName = "Postponed", ApiBasketballStatusCode = "POST" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Cancelled, FullGameStatusName = "Cancelled", GameStatusId = GameStatus.Cancelled, GameStatusName = "Cancelled", ApiBasketballStatusCode = "CANC" },
					new RefGameStatus{ FullGameStatusId = FullGameStatus.Unknown, FullGameStatusName = "Unknown", GameStatusId = GameStatus.Unknown, GameStatusName = "Unknown", ApiBasketballStatusCode = null }
				});
				context.SaveChanges();

				var bookmakersProcessor = new BookmakersProcessor();
				Console.WriteLine("START BOOKMAKERS");
				bookmakersProcessor.Run(context);
				Console.WriteLine("SAVE BOOKMAKERS");
				context.SaveChanges();
				Console.WriteLine("END BOOKMAKERS");

				var betTypesProcessor = new BetTypesProcessor();
				Console.WriteLine("START BET TYPES");
				betTypesProcessor.Run(context);
				Console.WriteLine("SAVE BET TYPES");
				context.SaveChanges();
				Console.WriteLine("END BET TYPES");

				var countriesProcessor = new CountriesProcessor();
				Console.WriteLine("START COUNTRIES");
				countriesProcessor.Run(context);
				Console.WriteLine("SAVE COUNTRIES");
				context.SaveChanges();
				Console.WriteLine("END COUNTRIES");

				var leaguesProcessor = new LeaguesProcessor();
				Console.WriteLine("START LEAGUES");
				leaguesProcessor.Run(context);
				Console.WriteLine("SAVE LEAGUES");
				context.SaveChanges();
				Console.WriteLine("END LEAGUES");

				var leagueSeasons = context.LeagueSeasons
											.OrderBy(x => x.LeagueSeasonId)
											.Select(y => new { y.ApiBasketballLeagueId, y.ApiBasketballSeasonKey })
											.ToList();

				//var desiredLeagueIds = new List<int> { 102, 74 };
				//leagueSeasons = leagueSeasons
				//.Where(x => desiredLeagueIds.Contains(x.ApiBasketballLeagueId))
				//.Where(x => x.SeasonEndUtc >= DateTime.UtcNow.Date)
				//.ToList();

				foreach (var leagueSeason in leagueSeasons)
				{
					int leagueId = leagueSeason.ApiBasketballLeagueId;
					string seasonKey = leagueSeason.ApiBasketballSeasonKey;
					var teamsProcessor = new TeamsProcessor(leagueId, seasonKey);
					Console.WriteLine($"START TEAMS - {leagueId} {seasonKey}");
					teamsProcessor.Run(context);
					Console.WriteLine($"SAVE TEAMS - {leagueId} {seasonKey}");
					context.SaveChanges();
					Console.WriteLine($"END TEAMS - {leagueId} {seasonKey}");
				}

				var countriesDict = context.Countries.ToDictionary(x => x.ApiBasketballCountryId, y => y.CountryId);
				var teamsDict = context.Teams.ToDictionary(x => x.ApiBasketballTeamId, y => y.TeamId);
				var statusDict = context.RefGameStatuses
										.Where(x => !string.IsNullOrEmpty(x.ApiBasketballStatusCode))
										.ToDictionary(x => x.ApiBasketballStatusCode, y => y.FullGameStatusId);
				foreach (var leagueSeason in leagueSeasons)
				{
					int leagueId = leagueSeason.ApiBasketballLeagueId;
					string seasonKey = leagueSeason.ApiBasketballSeasonKey;
					var gamesProcessor = new GamesProcessor(leagueId, seasonKey, countriesDict, teamsDict, statusDict);
					Console.WriteLine($"START GAMES - {leagueId} {seasonKey}");
					gamesProcessor.Run(context);
					Console.WriteLine($"SAVE GAMES - {leagueId} {seasonKey}");
					context.SaveChanges();
					Console.WriteLine($"END GAMES - {leagueId} {seasonKey}");
				}

				var bookmakersDict = context.Bookmakers.ToDictionary(x => x.ApiBasketballBookmakerId, y => y.BookmakerId);
				var betTypesDict = context.BetTypes.ToDictionary(x => x.ApiBasketballBetTypeId, y => y.BetTypeId);

				var liveAndUpcomingGameStatuses = context.RefGameStatuses
															.Where(x => x.GameStatusId == GameStatus.Pregame || x.GameStatusId == GameStatus.Live)
															.Select(x => x.FullGameStatusId)
															.ToList();
				var activeLeagueSeasons = context.Games
													.Where(x => liveAndUpcomingGameStatuses.Contains(x.FullGameStatusId)
																&& x.GameTimeUtc <= DateTime.UtcNow.Date.AddDays(4))
													.Select(y => new { y.LeagueSeason.ApiBasketballLeagueId, y.LeagueSeason.ApiBasketballSeasonKey })
													.Distinct()
													.ToList();
				foreach (var leagueSeason in leagueSeasons)
				{
					int leagueId = leagueSeason.ApiBasketballLeagueId;
					string seasonKey = leagueSeason.ApiBasketballSeasonKey;
					var oddsProcessor = new OddsProcessor(leagueId, seasonKey, betTypesDict, bookmakersDict);
					Console.WriteLine($"START ODDS - {leagueId} {seasonKey}");
					oddsProcessor.Run(context);
					Console.WriteLine($"SAVE ODDS - {leagueId} {seasonKey}");
					context.SaveChanges();
					Console.WriteLine($"END ODDS - {leagueId} {seasonKey}");
				}
			}
		}

		private static IConfiguration GetConfig()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			var config = builder.Build();
			return config;
		}
	}
}
