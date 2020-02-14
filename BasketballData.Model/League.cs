using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Model
{
	public class League
	{
		public int LeagueId { get; set; }
		public int? ParentLeagueId { get; set; }
		public string LeagueName { get; set; }
		public string LeagueLogo { get; set; }
		public string LeagueType { get; set; }
		public int CountryId { get; set; }
		public int ApiBasketballLeagueId { get; set; }

		public virtual Country Country { get; set; }
		public virtual List<LeagueSeason> LeagueSeasons { get; set; }
	}
}
