using System;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
	public class AnalyzableTick<T>
	{
		public DateTime? DateTime { get; }
		public T Tick { get; }

		public AnalyzableTick(DateTime? dateTime, T tick)
		{
			Tick = tick;
			DateTime = dateTime;
		}
	}
}
