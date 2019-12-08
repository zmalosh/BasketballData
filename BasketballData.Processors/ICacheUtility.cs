using System;

namespace BasketballData.Processors
{
	public interface ICacheUtility
	{
		bool ReadFile(string path, out string text, int? cacheTimeSeconds = null);
		void WriteFile(string path, string text);
	}
}
