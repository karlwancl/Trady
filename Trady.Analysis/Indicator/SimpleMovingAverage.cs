using System;
using System.Collections.Generic;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class SimpleMovingAverage<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        public int PeriodCount { get; }

        public SimpleMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount)
            : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => mappedInputs._Average(PeriodCount, index);
    }

    public class SimpleMovingAverageByTuple : SimpleMovingAverage<decimal, decimal?>
    {
        public SimpleMovingAverageByTuple(IEnumerable<decimal> values, int periodCount)
            : base(values, c => c, periodCount) { }
    }

    public class SimpleMovingAverage : SimpleMovingAverage<Candle, AnalyzableTick<decimal?>>
    {
        public SimpleMovingAverage(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount) { }
    }
}