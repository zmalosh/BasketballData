using BasketballData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Processors
{
	public class BookmakersProcessor : IProcessor
	{
		public void Run(BasketballDataContext dbContext)
		{
			var url = Feeds.BookmakersFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.BookmakersFeed.FromJson(rawJson);

			var existingBookmakers = dbContext.Bookmakers.ToDictionary(x => x.ApiBasketballBookmakerId, y => y);
			var apiBookmakers = feed.Bookmakers.ToList();

			foreach (var apiBookmaker in apiBookmakers)
			{
				if (!existingBookmakers.ContainsKey(apiBookmaker.Id))
				{
					var dbBookmaker = new Bookmaker
					{
						BookmakerName = apiBookmaker.Name,
						ApiBasketballBookmakerId = apiBookmaker.Id
					};
					existingBookmakers.Add(dbBookmaker.ApiBasketballBookmakerId, dbBookmaker);
					dbContext.Bookmakers.Add(dbBookmaker);
					dbContext.SaveChanges();
				}
			}
		}
	}
}
