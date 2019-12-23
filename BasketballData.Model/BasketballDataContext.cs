using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BasketballData.Model
{
	public class BasketballDataContext : DbContext
	{
		public BasketballDataContext() : base()
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();
			var connectionString = configuration["BasketballDataContextConnectionString"];
			optionsBuilder.UseSqlServer(connectionString);
		}

		public DbSet<Country> Countries { get; set; }
		public DbSet<League> Leagues { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Country>().HasKey(c => c.CountryId);
			modelBuilder.Entity<Country>().Property(c => c.CountryAbbr).HasMaxLength(2);

			modelBuilder.Entity<League>().HasKey(x => x.LeagueId);
			modelBuilder.Entity<League>().HasOne(x => x.Country).WithMany(y => y.Leagues).HasForeignKey(x => x.CountryId);
		}
	}
}
