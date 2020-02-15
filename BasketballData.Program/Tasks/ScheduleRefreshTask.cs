using BasketballData.Model;
using BasketballData.Processors.ApiBasketball.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketballData.Program.Tasks
{
	public class ScheduleRefreshTask : BaseTask
	{
		public override void Run()
		{
			var config = GetConfig();

			var context = new BasketballDataContext(config);

			var activeLeagueSeasons = context.LeagueSeasons.Where(x => x.IsActive).ToList();

			if (activeLeagueSeasons != null && activeLeagueSeasons.Count > 0)
			{
				var countriesDict = context.Countries.ToDictionary(x => x.ApiBasketballCountryId, y => y.CountryId);
				var teamsDict = context.Teams.ToDictionary(x => x.ApiBasketballTeamId, y => y.TeamId);
				var statusDict = context.RefGameStatuses
										.Where(x => !string.IsNullOrEmpty(x.ApiBasketballStatusCode))
										.ToDictionary(x => x.ApiBasketballStatusCode, y => y.FullGameStatusId);
				for (int i = 0; i < activeLeagueSeasons.Count; i++)
				{
					var leagueSeason = activeLeagueSeasons[i];

					var gamesProcessor = new GamesProcessor(leagueSeason.ApiBasketballLeagueId, leagueSeason.ApiBasketballSeasonKey, countriesDict, teamsDict, statusDict);
					gamesProcessor.Run(context);
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
