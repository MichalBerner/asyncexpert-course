using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
	[DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
	[MemoryDiagnoser]
	[NativeMemoryProfiler]
	public class FibonacciCalc
	{
		// HOMEWORK:
		// 1. Write implementations for RecursiveWithMemoization and Iterative solutions
		// 2. Add memory profilers (MemoryDiagnoser and NativeMemoryProfiler) to the benchmark
		// 3. Run with release configuration and compare results
		// 4. Open disassembler report and compare machine code
		// 
		// You can use the discussion panel to compare your results with other students

		private readonly Dictionary<ulong, ulong> _results = new Dictionary<ulong, ulong>()
		{
			[1] = 1,
			[2] = 1
		};

		[Benchmark(Baseline = true)]
		[ArgumentsSource(nameof(Data))]
		public ulong Recursive(ulong n)
		{
			if (n == 1 || n == 2) return 1;
			return Recursive(n - 2) + Recursive(n - 1);
		}

		[Benchmark]
		[ArgumentsSource(nameof(Data))]
		public ulong RecursiveWithMemoization(ulong n)
		{
			if (!_results.TryGetValue(n, out var result))
			{
				result = RecursiveWithMemoization(n - 2) + RecursiveWithMemoization(n - 1);
				_results.Add(n, result);
			}
			
			return result;
		}
		
		[Benchmark]
		[ArgumentsSource(nameof(Data))]
		public ulong Iterative(ulong n)
		{
			if (n == 1 || n == 2)
			{
				return 1;
			}

			var previous = 1ul;
			var beforePrevious = 1ul;
			var current = 0ul;

			for (ulong i = 3; i <= n; i++)
			{
				current = previous + beforePrevious;
				beforePrevious = previous;
				previous = current;
			}

			return current;
		}

		public IEnumerable<ulong> Data()
		{
			yield return 10;
			yield return 20;
			yield return 30;
		}
	}
}
