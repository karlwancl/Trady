using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Candlestick
{
    public class LongLowerShadow<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Low, decimal Close), bool?, TOutput>
    {
        public LongLowerShadow(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Low, decimal Close)> inputMapper, int periodCount = 20, decimal threshold = 0.25m) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }
        public decimal Threshold { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal Low, decimal Close)> mappedInputs, int index)
        {
            var lowerShadows = mappedInputs.Select(i => Math.Min(i.Open, i.Close) - i.Low);
            return lowerShadows.ElementAt(index) < lowerShadows._Percentile(PeriodCount, index, Threshold);
        }
    }

    public class LongLowerShadowByTuple : LongLowerShadow<(decimal Open, decimal Low, decimal Close), bool?>
    {
        public LongLowerShadowByTuple(IEnumerable<(decimal Open, decimal Low, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25M)
            : base(inputs, i => i, periodCount, threshold)
        {
        }
    }

    public class LongLowerShadow : LongLowerShadow<Candle, AnalyzableTick<bool?>>
    {
        public LongLowerShadow(IEnumerable<Candle> inputs, int periodCount = 20, decimal threshold = 0.25M)
            : base(inputs, i => (i.Open, i.Low, i.Close), periodCount, threshold)
        {
        }
    }
}