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
				context.Countries.Add(new Country { ApiBasketballCountryId = 123, CountryAbbr = "XX", CountryName = "XXXXXX", FlagUrl = "http://www.asdf.com" });
				context.SaveChanges();
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
