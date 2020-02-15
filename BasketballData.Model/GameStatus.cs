using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BasketballData.Model
{
	public enum FullGameStatus
	{
		NotStarted = 101,
		Live_Q1 = 201,
		Live_Q2 = 202,
		Live_Q3 = 203,
		Live_Q4 = 204,
		Live_OT = 205,
		Live_HT = 210,
		Live_BK = 211,
		Final = 301,
		FinalWithOT = 302,
		Postponed = 401,
		Cancelled = 501,
		Unknown = 999
	}

	public enum GameStatus
	{
		Pregame = 1,
		Live = 2,
		Final = 3,
		Postponed = 4,
		Cancelled = 5,
		Unknown = 9
	}

	public class RefGameStatus
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public FullGameStatus FullGameStatusId { get; set; }
		public GameStatus GameStatusId { get; set; }
		public string FullGameStatusName { get; set; }
		public string GameStatusName { get; set; }
		public string ApiBasketballStatusCode { get; set; }
		public DateTime DateLastModifiedUtc { get; set; }
		public DateTime DateCreatedUtc { get; set; }
	}
}
