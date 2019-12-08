using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Processors
{
	public interface IProcessor
	{
		void Run(Model.BasketballDataContext dbContext);
	}
}
