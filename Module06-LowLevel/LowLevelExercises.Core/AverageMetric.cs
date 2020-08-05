using System.Numerics;
using System.Threading;

namespace LowLevelExercises.Core
{
	/// <summary>
	/// A simple class for reporting a specific value and obtaining an average.
	/// </summary>
	/// TODO: remove the locking and use <see cref="Interlocked"/> and <see cref="Volatile"/> to implement a lock-free implementation.
	public class AverageMetric
	{
		private volatile int _sum = 0;
		private volatile int _count = 0;

		public void Report(int value)
		{
			Interlocked.Add(ref _sum, value);
			Interlocked.Increment(ref _count);
		}

		public double Average => Calculate(_count, _sum);

		private static double Calculate(in int count, in int sum)
		{
			// DO NOT change the way calculation is done.

			if (count == 0)
			{
				return double.NaN;
			}

			return (double)sum / count;
		}
	}
}
