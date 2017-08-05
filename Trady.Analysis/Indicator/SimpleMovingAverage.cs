using System;
using System.Collections.Generic;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class SimpleMovingAverage<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public int PeriodCount { get; }

        public SimpleMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount)
            : base(inputs, inputMapper, outputMapper)
        {
            PeriodCount = periodCount;
        }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
            => mappedInputs.Avg(PeriodCount, index);
    }

    public class SimpleMovingAverageByTuple : SimpleMovingAverage<decimal, decimal?>
    {
        public SimpleMovingAverageByTuple(IEnumerable<decimal> values, int periodCount)
            : base(values, c => c, (c, otm) => otm, periodCount) { }
    }

    public class SimpleMovingAverage : SimpleMovingAverage<Candle, AnalyzableTick<decimal?>>
    {
        public SimpleMovingAverage(IEnumerable<Candle> candles, int periodCount)
            : base(candles, c => c.Close, (c, otm) => new AnalyzableTick<decimal?>(c.DateTime, otm), periodCount) { }
    }
}
