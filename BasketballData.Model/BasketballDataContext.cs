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

		public DbSet<RefGameStatus> RefGameStatuses { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<League> Leagues { get; set; }
		public DbSet<LeagueSeason> LeagueSeasons { get; set; }
		public DbSet<Team> Teams { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RefGameStatus>().HasKey(x => x.FullGameStatusId);

			modelBuilder.Entity<Country>().HasKey(c => c.CountryId);
			modelBuilder.Entity<Country>().Property(c => c.CountryAbbr).HasMaxLength(2);
			modelBuilder.Entity<Country>().Property(x => x.FlagUrl).HasMaxLength(255);

			modelBuilder.Entity<League>().HasKey(x => x.LeagueId);
			modelBuilder.Entity<League>().HasOne(x => x.Country).WithMany(y => y.Leagues).HasForeignKey(x => x.CountryId);
			modelBuilder.Entity<League>().Property(x => x.LeagueLogo).HasMaxLength(255);

			modelBuilder.Entity<LeagueSeason>().HasKey(x => x.LeagueSeasonId);
			modelBuilder.Entity<LeagueSeason>().HasOne(x => x.League).WithMany(y => y.LeagueSeasons).HasForeignKey(x => x.LeagueId);

			modelBuilder.Entity<Team>().HasKey(x => x.TeamId);
			modelBuilder.Entity<Team>().HasOne(x => x.Country).WithMany(y => y.Teams).HasForeignKey(x => x.CountryId);
			modelBuilder.Entity<Team>().Property(x => x.TeamName).HasMaxLength(64);
			modelBuilder.Entity<Team>().Property(x => x.TeamLogoUrl).HasMaxLength(255);
		}
	}
}
