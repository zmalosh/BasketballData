using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Model
{
	public class Game : IEntity
	{
		public int GameId { get; set; }
		public int HomeTeamId { get; set; }
		public int AwayTeamId { get; set; }
		public int LeagueSeasonId { get; set; }
		public int CountryId { get; set; }
		public FullGameStatus FullGameStatusId { get; set; }
		public int? HomeScore { get; set; }
		public int? AwayScore { get; set; }
		public DateTime GameTimeUtc { get; set; }
		public int? QtrTimeRem { get; set; }
		public int? HomeScore_Q1 { get; set; }
		public int? HomeScore_Q2 { get; set; }
		public int? HomeScore_Q3 { get; set; }
		public int? HomeScore_Q4 { get; set; }
		public int? HomeScore_OT { get; set; }
		public int? AwayScore_Q1 { get; set; }
		public int? AwayScore_Q2 { get; set; }
		public int? AwayScore_Q3 { get; set; }
		public int? AwayScore_Q4 { get; set; }
		public int? AwayScore_OT { get; set; }
		public int ApiBasketballGameId { get; set; }
		public DateTime DateLastModifiedUtc { get; set; }
		public DateTime DateCreatedUtc { get; set; }

		public virtual LeagueSeason LeagueSeason { get; set; }
		public virtual Country Country { get; set; }
		public virtual Team HomeTeam { get; set; }
		public virtual Team AwayTeam { get; set; }
		public virtual IList<BetLine> BetLines { get; set; }
	}
}
