using BasketballData.Model;
using BasketballData.Processors.ApiBasketball.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketballData.Program.Tasks
{
	public class OddsUpdateTask : BaseTask
	{
		public override void Run()
		{
			var config = GetConfig();

			var context = new BasketballDataContext(config);

			// UPDATE ODDS FOR GAMES STARTING IN NEXT 24 HOURS
			var targetedApiBasketballLeagueSeasonInfos = context.Games.Where(x => x.LeagueSeason.IsActive)
																		.Select(x => new { x.LeagueSeason.ApiBasketballLeagueId, x.LeagueSeason.ApiBasketballSeasonKey })
																		.Distinct()
																		.ToList();

			if (targetedApiBasketballLeagueSeasonInfos != null && targetedApiBasketballLeagueSeasonInfos.Count > 0)
			{
				var bookmakersDict = context.Bookmakers.ToDictionary(x => x.ApiBasketballBookmakerId, y => y.BookmakerId);
				var betTypesDict = context.BetTypes.ToDictionary(x => x.ApiBasketballBetTypeId, y => y.BetTypeId);

				for (int i = 0; i < targetedApiBasketballLeagueSeasonInfos.Count; i++)
				{
					var leagueSeasonInfo = targetedApiBasketballLeagueSeasonInfos[i];
					var oddsProcessor = new OddsProcessor(leagueSeasonInfo.ApiBasketballLeagueId, leagueSeasonInfo.ApiBasketballSeasonKey, betTypesDict, bookmakersDict);
					oddsProcessor.Run(context);
					context.SaveChanges();

					if (i % 10 == 9)
					{
						context.Dispose();
						context = new BasketballDataContext(config);
					}
				}
			}

			context.Dispose();
		}
	}
}
