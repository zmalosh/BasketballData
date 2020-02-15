using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Program
{
	public class CommandLineOptions
	{
		[Option('i', "initializeFullLoad", Required = false, HelpText = "Initialize database in full.")]
		public bool InitializeFullLoadTask { get; set; }

		[Option('f', "frequent", Required = false, HelpText = "Run update task created to run on frequent basis. Includes game and odds updates for selected leagues based on game schedule.")]
		public bool FrequentUpdateTask { get; set; }

		[Option('o', "odds", Required = false, HelpText = "Run task to update odds for all active league seasons")]
		public bool OddsUpdateTask { get; set; }

		[Option('s', "schedule", Required = false, HelpText = "Run task to refresh all active seasons")]
		public bool ScheduleRefreshTask { get; set; }
	}
}
