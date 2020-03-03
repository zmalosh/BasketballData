using BasketballData.Model;
using BasketballData.Processors.ApiBasketball.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketballData.Program.Tasks
{
	public class FrequentUpdateTask : BaseTask
	{
		private static readonly List<FullGameStatus> liveOrFinalGameStatuses = new List<FullGameStatus> {
			FullGameStatus.Final, FullGameStatus.FinalWithOT, FullGameStatus.Live_BK,
			FullGameStatus.Live_HT, FullGameStatus.Live_OT, FullGameStatus.Live_Q1,
			FullGameStatus.Live_Q2, FullGameStatus.Live_Q3, FullGameStatus.Live_Q4
		};
		private static readonly List<FullGameStatus> liveOrPregameGameStatuses = new List<FullGameStatus> {
			FullGameStatus.NotStarted, FullGameStatus.Live_BK,
			FullGameStatus.Live_HT, FullGameStatus.Live_OT, FullGameStatus.Live_Q1,
			FullGameStatus.Live_Q2, FullGameStatus.Live_Q3, FullGameStatus.Live_Q4
		};

		public override void Run()
		{
			var config = GetConfig();

			using (var context = new BasketballDataContext(config))
			{
				// ASSUME ALL GAMES ARE IN DB FROM DAILY TASK

				// UPDATE RESULTS FOR LIVE/RECENTLY FINAL GAMES
				// SELECT LEAGUE SEASONS WHERE
				// 1) A GAME HAS STARTED SINCE THE LAST UNIVERSAL GAME HAS GONE FINAL (PICKUP GAMES AFTER PERIOD WITHOUT RUNNING)
				// 2) A GAME IS LIVE
				// 3) THE GAME WILL START IN THE NEXT 4 HOURS
				DateTime lastFinalGameStart = context.Games.Where(x => liveOrFinalGameStatuses.Contains(x.FullGameStatusId)).Max(x => x.GameTimeUtc);
				var targetedApiBasketballLeagueSeasonInfos = context.Games.Where(x => x.LeagueSeason.IsActive
																						&& x.GameTimeUtc > lastFinalGameStart
																						&& liveOrPregameGameStatuses.Contains(x.FullGameStatusId)
																						&& x.GameTimeUtc <= DateTime.UtcNow.AddHours(4))
																			.Select(x => new { x.LeagueSeason.ApiBasketballLeagueId, x.LeagueSeason.ApiBasketballSeasonKey })
																			.Distinct()
																			.ToList();

				if (targetedApiBasketballLeagueSeasonInfos != null && targetedApiBasketballLeagueSeasonInfos.Count > 0)
				{
					var countriesDict = context.Countries.ToDictionary(x => x.ApiBasketballCountryId, y => y.CountryId);
					var statusDict = context.RefGameStatuses
											.Where(x => !string.IsNullOrEmpty(x.ApiBasketballStatusCode))
											.ToDictionary(x => x.ApiBasketballStatusCode, y => y.FullGameStatusId);

					foreach (var leagueSeasonInfo in targetedApiBasketballLeagueSeasonInfos)
					{
						var gamesProcessor = new GamesProcessor(leagueSeasonInfo.ApiBasketballLeagueId, leagueSeasonInfo.ApiBasketballSeasonKey, countriesDict, statusDict);
						gamesProcessor.Run(context);
						context.SaveChanges();
					}
				}
			}
		}
	}
}
