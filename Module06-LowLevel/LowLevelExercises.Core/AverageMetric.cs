using System.Threading;

namespace LowLevelExercises.Core
{
	/// <summary>
	/// A simple class for reporting a specific value and obtaining an average.
	/// </summary>
	public class AverageMetric
	{
		private int _sum = 0;
		private int _count = 0;
		private int _isWorking;

		public void Report(int value)
		{
			var spinner = new SpinWait();
			
			while (true)
			{
				if (Interlocked.CompareExchange(ref _isWorking, 1, 0) == 0)
				{
					_sum += value;
					Volatile.Write(ref _count, _count + 1);
					Interlocked.Exchange(ref _isWorking, 0);
					
					return;
				}

				spinner.SpinOnce();
			}
		}

		public double Average
		{
			get
			{
				var spinner = new SpinWait();

				while (true)
				{
					if (Interlocked.CompareExchange(ref _isWorking, 1, 0) == 0)
					{
						var calculationResult = Calculate(Volatile.Read(ref _count), _sum);
						Interlocked.Exchange(ref _isWorking, 0);

						return calculationResult;
					}
					
					spinner.SpinOnce();
				}
			}
		}

		private static double Calculate(in int count, in int sum)
		{
			if (count == 0)
			{
				return double.NaN;
			}

			return (double)sum / count;
		}
	}
}
