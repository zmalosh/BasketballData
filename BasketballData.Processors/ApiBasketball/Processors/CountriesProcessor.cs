using BasketballData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Processors
{
	public class CountriesProcessor : IProcessor
	{
		public void Run(BasketballDataContext dbContext)
		{
			var url = Feeds.CountriesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.CountriesFeed.FromJson(rawJson);

			var orderedCountries = feed.Countries
										.OrderBy(x => x.Code)
										.ToList();

			var existingCountries = dbContext.Countries.ToDictionary(x => x.CountryAbbr ?? "(null)");

			foreach (var country in orderedCountries)
			{
				if (!existingCountries.ContainsKey(country.Code ?? "(null)"))
				{
					var dbCountry = new Country
					{
						CountryName = country.Name,
						CountryAbbr = country.Code,
						FlagUrl = country.Flag?.ToString(),
						ApiBasketballCountryId = country.Id
					};
					existingCountries.Add(country.Code, dbCountry);
					dbContext.Countries.Add(dbCountry);
				}
			}
		}
	}
}
