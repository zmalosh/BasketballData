﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Model
{
	public class Country
	{
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public string CountryAbbr { get; set; }
		public string FlagUrl { get; set; }
		public int ApiBasketballCountryId { get; set; }
	}
}