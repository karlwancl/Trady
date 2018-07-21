using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class Lowest<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        public Lowest(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
            => index >= PeriodCount - 1 ? mappedInputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Min() : default;
    }

    public class LowestByTuple : Lowest<decimal?, decimal?>
    {
        public LowestByTuple(IEnumerable<decimal?> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }

		public LowestByTuple(IEnumerable<decimal> inputs, int periodCount)
	        : this(inputs.Cast<decimal?>(), periodCount)
		{
		}
    }
}
