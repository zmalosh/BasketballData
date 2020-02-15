using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Program
{
	public class CommandLineOptions
	{
		[Option('i', "initializeFullLoad", Required = false, HelpText = "Initialize database in full.")]
		public bool InitializeFullLoad { get; set; }
	}
}
