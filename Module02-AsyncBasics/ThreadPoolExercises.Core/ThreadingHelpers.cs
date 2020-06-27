using System;
using System.Threading;

namespace ThreadPoolExercises.Core
{
	public class ThreadingHelpers
	{
		public static void ExecuteOnThread(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
		{
			// * Create a thread and execute there `action` given number of `repeats` - waiting for the execution!
			//   HINT: you may use `Join` to wait until created Thread finishes
			// * In a loop, check whether `token` is not cancelled
			// * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)

			var thread = new Thread(() => ExecuteWithExceptionHandling(action, repeats, token, errorAction));
			thread.Start();
			thread.Join();
		}

		public static void ExecuteOnThreadPool(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
		{
			// * Queue work item to a thread pool that executes `action` given number of `repeats` - waiting for the execution!
			//   HINT: you may use `AutoResetEvent` to wait until the queued work item finishes
			// * In a loop, check whether `token` is not cancelled
			// * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)

			var autoResetEvent = new AutoResetEvent(false);
			ThreadPool.QueueUserWorkItem(o => ExecuteWithExceptionHandling(action, repeats, token, errorAction, autoResetEvent));
			autoResetEvent.WaitOne();
		}

		private static void ExecuteWithExceptionHandling(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null, AutoResetEvent? autoResetEvent = null)
		{
			try
			{
				if (repeats == 0)
				{
					return;
				}

				for (int i = 0; i < repeats; i++)
				{
					token.ThrowIfCancellationRequested();
					action.Invoke();
				}
			}
			catch (Exception e)
			{
				errorAction?.Invoke(e);
			}
			finally
			{
				autoResetEvent?.Set();
			}
		}
	}
}
