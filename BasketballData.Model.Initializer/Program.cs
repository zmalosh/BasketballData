using BasketballData.Processors.ApiBasketball.Processors;
using Microsoft.Extensions.Configuration;
using System;
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
				context.SaveChanges();

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
