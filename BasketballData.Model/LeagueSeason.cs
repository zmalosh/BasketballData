using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Model
{
	public class LeagueSeason
	{
		public int LeagueSeasonId { get; set; }
		public int LeagueId { get; set; }
		public DateTime SeasonStartUtc { get; set; }
		public DateTime SeasonEndUtc { get; set; }
		public int ApiBasketballLeagueId { get; set; }
		public string ApiBasketballSeasonKey { get; set; }
		public bool IsActive { get; private set; }
		public DateTime DateLastModifiedUtc { get; set; }
		public DateTime DateCreatedUtc { get; set; }

		public virtual League League { get; set; }
		public virtual IList<Game> Games { get; set; }
	}
}
