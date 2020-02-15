using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Model
{
	public class Bookmaker : IEntity
	{
		public int BookmakerId { get; set; }
		public string BookmakerName { get; set; }
		public int ApiBasketballBookmakerId { get; set; }
		public DateTime DateLastModifiedUtc { get; set; }
		public DateTime DateCreatedUtc { get; set; }

		public virtual IList<BetLine> BetLines { get; set; }
	}
}
