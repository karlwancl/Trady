using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class Highest<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        public Highest(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
            => index >= PeriodCount - 1 ? mappedInputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Max() : default;
    }

    public class HighestByTuple : Highest<decimal?, decimal?>
    {
        public HighestByTuple(IEnumerable<decimal?> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }

		public HighestByTuple(IEnumerable<decimal> inputs, int periodCount)
	        : this(inputs.Cast<decimal?>(), periodCount)
		{
		}
    }
}
