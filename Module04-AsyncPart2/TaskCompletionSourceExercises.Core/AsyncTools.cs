using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskCompletionSourceExercises.Core
{
	public class AsyncTools
	{
		public static Task<string> RunProgramAsync(string path, string args = "")
		{
			var process = new Process
			{
				EnableRaisingEvents = true,
				StartInfo = new ProcessStartInfo(path, args)
				{
					RedirectStandardOutput = true, RedirectStandardError = true
				}
			};

			var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
			
			process.Exited += async (sender, eventArgs) =>
			{
				var senderProcess = sender as Process;

				if (process.ExitCode == 0)
				{
					var output = await senderProcess.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
					tcs.SetResult(output);
				}
				else
				{
					var error = await senderProcess.StandardError.ReadToEndAsync().ConfigureAwait(false);
					tcs.SetException(new Exception(error));
				}
				
				senderProcess?.Dispose();
			};

			process.Start();
			
			return tcs.Task;
		}
	}
}
