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
					if (o.FrequentUpdateTask)
					{
						task = new Tasks.FrequentUpdateTask();
					}
					else if (o.OddsUpdateTask)
					{
						task = new Tasks.OddsUpdateTask();
					}
					else if (o.ScheduleRefreshTask)
					{
						task = new Tasks.ScheduleRefreshTask();
					}
					else if (o.InitializeFullLoadTask)
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
