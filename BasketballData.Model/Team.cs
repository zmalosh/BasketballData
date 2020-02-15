using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Model
{
	public class Team
	{
		public int TeamId { get; set; }
		public string TeamName { get; set; }
		public string TeamLogoUrl { get; set; }
		public bool IsNationalTeam { get; set; }
		public int CountryId { get; set; }
		public int ApiBasketballTeamId { get; set; }
		public DateTime DateLastModifiedUtc { get; set; }
		public DateTime DateCreatedUtc { get; set; }

		public virtual Country Country { get; set; }
		public virtual IList<Game> HomeGames { get; set; }
		public virtual IList<Game> AwayGames { get; set; }
	}
}
