using BasketballData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Processors
{
	public class TeamsProcessor : IProcessor
	{
		private readonly int apiBasketballLeagueId;
		private readonly string apiBasketballSeasonKey;

		public TeamsProcessor(int apiBasketballLeagueId, string apiBasketballSeasonKey)
		{
			this.apiBasketballLeagueId = apiBasketballLeagueId;
			this.apiBasketballSeasonKey = apiBasketballSeasonKey;
		}

		public void Run(BasketballDataContext dbContext)
		{
			var url = Feeds.TeamsFeed.GetFeedUrl(this.apiBasketballLeagueId, this.apiBasketballSeasonKey);
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.TeamsFeed.FromJson(rawJson);

			// ASSUME COUNTRY ALREADY EXISTS IN COUNTRIES API ENDPOINT
			var countriesDict = dbContext.Countries.ToDictionary(x => x.ApiBasketballCountryId, y => y.CountryId);

			var existingTeams = dbContext.Teams.ToDictionary(x => x.ApiBasketballTeamId, y => y);

			var apiTeams = feed.Teams.ToList();

			foreach (var apiTeam in apiTeams)
			{
				if (!existingTeams.TryGetValue(apiTeam.Id, out Team dbTeam))
				{
					dbTeam = new Team
					{
						ApiBasketballTeamId = apiTeam.Id,
						CountryId = countriesDict[apiTeam.Country.Id],
						IsNationalTeam = apiTeam.IsNational,
						TeamLogoUrl = apiTeam.Logo,
						TeamName = apiTeam.Name
					};

					existingTeams.Add(dbTeam.ApiBasketballTeamId, dbTeam);
					dbContext.Teams.Add(dbTeam);
				}
			}
		}
	}
}
