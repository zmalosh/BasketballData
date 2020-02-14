using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace BasketballData.Processors.ApiBasketball
{
	public static class JsonUtility
	{
		private static readonly string ApiKey = File.ReadAllText("ApiBasketball.key");
		private static readonly WebClient WebClient = CreateWebClient();
		// private static readonly ICacheUtility CacheUtility = new AzureUtility();
		private static readonly ICacheUtility CacheUtility = new NoCacheUtility();

		public static string GetRawJsonFromUrl(string url, int? cacheTimeSeconds = null)
		{
			string cachePath = GetCachePathFromUrl(url);

			if (!cacheTimeSeconds.HasValue || !CacheUtility.ReadFile(cachePath, out string rawJson, cacheTimeSeconds))
			{
				try
				{
					rawJson = WebClient.DownloadString(url);
				}
				catch
				{
					return null;
				}
				if (!cacheTimeSeconds.HasValue || (cacheTimeSeconds.HasValue && cacheTimeSeconds.Value > 0))
				{
					CacheUtility.WriteFile(cachePath, rawJson);
				}
			}

			return rawJson;
		}

		private static string GetCachePathFromUrl(string url)
		{
			var rawPath = url.Split(".com/")[1];
			var path = rawPath.Replace("/", "_");
			return path;
		}

		private static WebClient CreateWebClient()
		{
			var client = new WebClient();
			client.Headers.Add("x-rapidapi-host", "api-basketball.p.rapidapi.com");
			client.Headers.Add("x-rapidapi-key", ApiKey);
			return client;
		}
	}
}
