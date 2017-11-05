using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
	public class PercentageDifference<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
	{
        public PercentageDifference(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount = 1) : base(inputs, inputMapper)
		{
            PeriodCount = periodCount;
		}

        public int PeriodCount { get; }

		protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
            => index >= PeriodCount ? (mappedInputs[index] - mappedInputs[index - PeriodCount]) / mappedInputs[index - PeriodCount] * 100 : (decimal?)null;
	}

	public class PercentageDifferenceByTuple : PercentageDifference<decimal?, decimal?>
	{
        public PercentageDifferenceByTuple(IEnumerable<decimal?> inputs, int periodCount = 1)
			: base(inputs, i => i, periodCount)
		{
		}

        public PercentageDifferenceByTuple(IEnumerable<decimal> inputs, int periodCount = 1)
	        : this(inputs.Cast<decimal?>(), periodCount)
		{
		}
	}
}
