using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasketballData.Model;

namespace BasketballData.Processors.ApiBasketball.Processors
{
	public class LeaguesProcessor : IProcessor
	{
		public void Run(BasketballDataContext dbContext)
		{
			var url = Feeds.LeaguesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.LeaguesFeed.FromJson(rawJson);

			// ASSUME COUNTRY ALREADY EXISTS IN COUNTRIES API ENDPOINT
			var countriesDict = dbContext.Countries.ToDictionary(x => x.ApiBasketballCountryId, y => y.CountryId);

			var existingLeagues = dbContext.Leagues.ToDictionary(x => x.ApiBasketballLeagueId, y => y);

			var orderedLeagues = feed.Leagues.OrderBy(x => x.Country.Code ?? string.Empty).ThenBy(y => y.Name).ToList();

			foreach (var apiLeague in orderedLeagues)
			{
				if (!existingLeagues.TryGetValue(apiLeague.Id, out League dbLeague))
				{
					dbLeague = new League
					{
						ApiBasketballLeagueId = apiLeague.Id,
						LeagueName = apiLeague.Name,
						LeagueLogo = apiLeague.Logo,
						CountryId = 1, //countriesDict[apiLeague.Country.Id],
						LeagueType = apiLeague.Type
					};

					existingLeagues.Add(dbLeague.ApiBasketballLeagueId, dbLeague);
					dbContext.Leagues.Add(dbLeague);
				}
			}
		}
	}
}
