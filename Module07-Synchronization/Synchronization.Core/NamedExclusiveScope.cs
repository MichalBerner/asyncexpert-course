using System;
using System.Threading;

namespace Synchronization.Core
{
	/*
	 * Implement very simple wrapper around Mutex or Semaphore (remember both implement WaitHandle) to
	 * provide a exclusive region created by `using` clause.
	 *
	 * Created region may be system-wide or not, depending on the constructor parameter.
	 */
	public class NamedExclusiveScope : IDisposable
	{ 
		private readonly Mutex _mutex;

		public NamedExclusiveScope(string name, bool isSystemWide)
		{
			_mutex = new Mutex(true, isSystemWide ? $"Global\\{name}" : name, out var isCreatedNew);
			
			if (!isCreatedNew)
			{
				_mutex = null;
				throw new InvalidOperationException($"Unable to get a global lock {name}.");
			}

			_mutex.WaitOne();
		}

		public void Dispose()
		{
			_mutex?.ReleaseMutex();
			_mutex?.Dispose();
		}
	}
}
