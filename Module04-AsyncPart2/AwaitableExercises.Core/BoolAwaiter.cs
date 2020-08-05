using System;
using System.Runtime.CompilerServices;

namespace AwaitableExercises.Core
{
	public static class BoolExtensions
	{
		public static BoolAwaiter GetAwaiter(this bool value)
		{
			return new BoolAwaiter(value);
		}

		//public static TaskAwaiter<bool> GetAwaiter(this bool value)
		//{
		//	return Task.FromResult(value).GetAwaiter();
		//}
	}

	public class BoolAwaiter : INotifyCompletion
	{
		private bool _value;

		public BoolAwaiter(bool value)
		{
			_value = value;
		}

		public bool IsCompleted { get; } = true;
		
		public void OnCompleted(Action continuation)
		{
			throw new NotImplementedException();
		}

		public bool GetResult()
		{
			return _value;
		}
	}
}
