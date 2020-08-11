using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DataStructures
{
	public class ConcurrentDictionaryWithCounterMetricsCounter : IMetricsCounter
	{
		// Implement this class using ConcurrentDictionary and the provided AtomicCounter class.
		// AtomicCounter should be created only once per key, then its Increment method should be used.

		private readonly ConcurrentDictionary<string, AtomicCounter> _counters;

		public ConcurrentDictionaryWithCounterMetricsCounter()
		{
			_counters = new ConcurrentDictionary<string, AtomicCounter>();
		}

		public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
		{
			return _counters.Select(c => new KeyValuePair<string ,int>(c.Key, c.Value.Count)).GetEnumerator();
		}

		public void Increment(string key)
		{
			_counters.GetOrAdd(key, _ => new AtomicCounter()).Increment();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public class AtomicCounter
		{
			int value;

			public void Increment() => Interlocked.Increment(ref value);

			public int Count => Volatile.Read(ref value);
		}
	}
}