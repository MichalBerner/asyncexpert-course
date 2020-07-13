using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskCompletionSourceExercises.Core
{
	public class AsyncTools
	{
		public static Task<string> RunProgramAsync(string path, string args = "")
		{
			var process = new Process();
			process.EnableRaisingEvents = true;
			process.StartInfo = new ProcessStartInfo(path, args)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};

			var tcs = new TaskCompletionSource<string>();
			
			process.Exited += async (sender, eventArgs) =>
			{
				var senderProcess = sender as Process;

				var error = await senderProcess.StandardError.ReadToEndAsync().ConfigureAwait(false);

				if (!string.IsNullOrEmpty(error))
				{
					tcs.SetException(new Exception(error));
				}
				else
				{
					var output = await senderProcess.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
					tcs.SetResult(output);
				}

				senderProcess?.Dispose();
			};

			process.Start();
			
			return tcs.Task;
		}
	}
}
