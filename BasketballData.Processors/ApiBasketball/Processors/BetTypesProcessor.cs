using BasketballData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Processors
{
	public class BetTypesProcessor : IProcessor
	{
		public void Run(BasketballDataContext dbContext)
		{
			var url = Feeds.BetTypesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.BetTypesFeed.FromJson(rawJson);

			var existingBookmakers = dbContext.BetTypes.ToDictionary(x => x.ApiBasketballBetTypeId, y => y);
			var apiBetTypes = feed.BetTypes.OrderBy(x => x.Id).ToList();

			foreach (var apiBetType in apiBetTypes)
			{
				if (!existingBookmakers.ContainsKey(apiBetType.Id))
				{
					var dbBetType = new BetType
					{
						BetTypeName = apiBetType.Name,
						ApiBasketballBetTypeId = apiBetType.Id
					};
					existingBookmakers.Add(dbBetType.ApiBasketballBetTypeId, dbBetType);
					dbContext.BetTypes.Add(dbBetType);
					dbContext.SaveChanges(); // NORMALLY DON'T SAVE IN PROCESSORS; ORDER OF BETTYPES IS BASED ON BET FREQUENCY, SO TRY TO PRESERVE ORDER FROM API
				}
			}
		}
	}
}
