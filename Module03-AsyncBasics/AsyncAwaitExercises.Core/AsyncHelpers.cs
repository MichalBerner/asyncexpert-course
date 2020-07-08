using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace AsyncAwaitExercises.Core
{
	public class AsyncHelpers
	{
		private const int MinTries = 2;
		private const int InitialDelay = 1000;

		public static Task<string> GetStringWithRetries(HttpClient client, string url, int maxTries = 3, CancellationToken token = default)
		{
			// Create a method that will try to get a response from a given `url`, retrying `maxTries` number of times.
			// It should wait one second before the second try, and double the wait time before every successive retry
			// (so pauses before retries will be 1, 2, 4, 8, ... seconds).
			// * `maxTries` must be at least 2
			// * we retry if:
			//    * we get non-successful status code (outside of 200-299 range), or
			//    * HTTP call thrown an exception (like network connectivity or DNS issue)
			// * token should be able to cancel both HTTP call and the retry delay
			// * if all retries fails, the method should throw the exception of the last try
			// HINTS:
			// * `HttpClient.GetAsync` does not accept cancellation token (use `GetAsync` instead)
			// * you may use `EnsureSuccessStatusCode()` method

			if (client is null)
			{
				throw new ArgumentNullException(nameof(client));
			}

			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentException(nameof(url));
			}

			if (maxTries < MinTries)
			{
				throw new ArgumentException(nameof(maxTries));
			}

			return GetStringWithRetriesInternal(client, url, maxTries, token);
		}

		private static async Task<string> GetStringWithRetriesInternal(HttpClient client, string url, int maxTries = 3,
			CancellationToken token = default)
		{
			var delay = 0;
			
			for (var i = 0; i < maxTries; i++)
			{
				await Task.Delay(TimeSpan.FromSeconds(delay), token);

				try
				{
					var httpResponse = await client.GetAsync(new Uri(url), token);
					httpResponse.EnsureSuccessStatusCode();
					return await httpResponse.Content.ReadAsStringAsync();
				}
				catch (HttpRequestException)
				{
					//ok. no problem.
				}

				delay = 1 << i;
			}

			throw new HttpRequestException("All tries failed");
		}
	}
}
