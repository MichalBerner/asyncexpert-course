using System.Collections.Generic;
using System.Threading.Tasks;
using LowLevelExercises.Core;
using NUnit.Framework;

namespace LowLevelExercises.Tests
{
	public class AverageMetricTests
	{
		[Test]
		public void Returns_NaN_when_nothing()
		{
			Assert.AreEqual(double.NaN, new AverageMetric().Average);
		}

		[Test]
		public void Returns_Average()
		{
			var metric = new AverageMetric();
			metric.Report(1);
			metric.Report(3);
			Assert.AreEqual(2, metric.Average);
		}

		[Test]
		public async Task Returns_AverageAsync()
		{
			var metric = new AverageMetric();
			
			var readers = new List<Task>();
			var tasks = new List<Task>();

			for (int i = 0; i < 10000; ++i)
			{
				tasks.Add(Task.Run(() => metric.Report(2)));
				
				if (i % 10 == 0)
				{
					readers.Add(Task.Run(() => Assert.AreEqual(2, metric.Average)));
				}
			}

			await Task.WhenAll(tasks);
			Assert.AreEqual(2, metric.Average);
			await Task.WhenAll(readers);
		}
	}
}