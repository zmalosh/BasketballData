using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BasketballData.Program
{
	public abstract class BaseTask
	{
		public abstract void Run();

		internal IConfiguration GetConfig()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			var config = builder.Build();
			return config;
		}
	}
}
