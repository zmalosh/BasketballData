using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Model
{
	public class Country : IEntity
	{
		public const int ApiBasketballWorldCountryId = 0;

		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public string CountryAbbr { get; set; }
		public string FlagUrl { get; set; }
		public int ApiBasketballCountryId { get; set; }
		public DateTime DateLastModifiedUtc { get; set; }
		public DateTime DateCreatedUtc { get; set; }

		public virtual List<League> Leagues { get; set; }
		public virtual List<Team> Teams { get; set; }
		public virtual List<Game> Games { get; set; }
	}
}
