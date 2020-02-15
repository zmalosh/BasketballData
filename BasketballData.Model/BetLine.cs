using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Model
{
	public class BetLine
	{
		public int BetLineId { get; set; }
		public int GameId { get; set; }
		public int BetTypeId { get; set; }
		public int BookmakerId { get; set; }
		public string BetName { get; set; }
		public decimal? BetValue { get; set; }
		public decimal? Line { get; set; }
		public DateTime DateLastModifiedUtc { get; set; }
		public DateTime DateCreatedUtc { get; set; }
		public virtual Game Game { get; set; }
		public virtual BetType BetType { get; set; }
		public virtual Bookmaker Bookmaker { get; set; }
	}
}
