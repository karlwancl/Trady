using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
	public class PercentDiff<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
	{
        public PercentDiff(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int numberOfDays = 1) : base(inputs, inputMapper)
		{
            NumberOfDays = numberOfDays;
		}

		public int NumberOfDays { get; }

		protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => index >= NumberOfDays ? (mappedInputs[index] - mappedInputs[index - NumberOfDays]) / mappedInputs[index - NumberOfDays] * 100 : (decimal?)null;
	}

	public class PercentDiffByTuple : PercentDiff<decimal, decimal?>
	{
		public PercentDiffByTuple(IEnumerable<decimal> inputs, int periodCount = 1)
			: base(inputs, i => i, periodCount)
		{
		}
	}
}
