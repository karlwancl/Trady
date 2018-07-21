using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class SimpleMovingAverage<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        public int PeriodCount { get; }

        public SimpleMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount)
            : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
            => index >= PeriodCount - 1 ? mappedInputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Average() : default;
    }

    public class SimpleMovingAverageByTuple : SimpleMovingAverage<decimal?, decimal?>
    {
        public SimpleMovingAverageByTuple(IEnumerable<decimal?> values, int periodCount)
            : base(values, c => c, periodCount) { }

		public SimpleMovingAverageByTuple(IEnumerable<decimal> values, int periodCount)
	        : this(values.Cast<decimal?>(), periodCount) { }
    }

    public class SimpleMovingAverage : SimpleMovingAverage<IOhlcv, AnalyzableTick<decimal?>>
    {
        public SimpleMovingAverage(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount) { }
    }
}
