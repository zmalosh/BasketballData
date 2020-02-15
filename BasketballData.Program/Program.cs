using CommandLine;
using System;

namespace BasketballData.Program
{
	class Program
	{
		static void Main(string[] args)
		{
			BaseTask task = null;

			Parser.Default.ParseArguments<CommandLineOptions>(args)
				.WithParsed(o =>
				{
					if (o.InitializeFullLoad)
					{
						task = new Tasks.InitializeFullLoadTask();
					}
				});

			if (task != null)
			{
				task.Run();
			}
		}
	}
}
