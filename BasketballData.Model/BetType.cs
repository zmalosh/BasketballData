using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Model
{
	public class BetType
	{
		public int BetTypeId { get; set; }
		public string BetTypeName { get; set; }
		public int ApiBasketballBetTypeId { get; set; }

		public virtual IList<BetLine> BetLines { get; set; }
	}
}
