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
		public DbSet<Bookmaker> Bookmakers { get; set; }
		public DbSet<BetType> BetTypes { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<League> Leagues { get; set; }
		public DbSet<LeagueSeason> LeagueSeasons { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Game> Games { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RefGameStatus>().HasKey(x => x.FullGameStatusId);
			modelBuilder.Entity<RefGameStatus>().Property(x => x.FullGameStatusName).HasMaxLength(32);
			modelBuilder.Entity<RefGameStatus>().Property(x => x.GameStatusName).HasMaxLength(32);
			modelBuilder.Entity<RefGameStatus>().Property(x => x.ApiBasketballStatusCode).HasMaxLength(4);

			modelBuilder.Entity<Bookmaker>().HasKey(x => x.BookmakerId);
			modelBuilder.Entity<Bookmaker>().Property(x => x.BookmakerName).HasMaxLength(32);

			modelBuilder.Entity<BetType>().HasKey(x => x.BetTypeId);
			modelBuilder.Entity<BetType>().Property(x => x.BetTypeName).HasMaxLength(128);

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

			modelBuilder.Entity<Game>().HasKey(x => x.GameId);
			modelBuilder.Entity<Game>().HasOne(x => x.Country).WithMany(y => y.Games).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Game>().HasOne(x => x.LeagueSeason).WithMany(y => y.Games).HasForeignKey(x => x.LeagueSeasonId).OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Game>().HasOne(x => x.HomeTeam).WithMany(y => y.HomeGames).HasForeignKey(x => x.HomeTeamId).OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Game>().HasOne(x => x.AwayTeam).WithMany(y => y.AwayGames).HasForeignKey(x => x.AwayTeamId).OnDelete(DeleteBehavior.Restrict);
		}
	}
}
