using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Processors.ApiBasketball.Feeds
{
	public class ApiCountry
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("flag")]
		public string Flag { get; set; }
	}
}
