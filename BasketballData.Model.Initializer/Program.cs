using BasketballData.Processors.ApiBasketball.Processors;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

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
