using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class LongLowerShadow<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Low, decimal Close), bool?, TOutput>
    {
        public LongLowerShadow(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Low, decimal Close)> inputMapper, Func<TInput, bool?, TOutput> outputMapper, int periodCount = 20, decimal threshold = 0.25m) : base(inputs, inputMapper, outputMapper)
        {
            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }
        public decimal Threshold { get; }

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal Open, decimal Low, decimal Close)> mappedInputs, int index)
        {
            var lowerShadows = mappedInputs.Select(i => Math.Min(i.Open, i.Close) - i.Low);
			return lowerShadows.ElementAt(index) < lowerShadows.Percentile(PeriodCount, index, Threshold);
        }
    }

    public class LongLowerShadowByTuple : LongLowerShadow<(decimal Open, decimal Low, decimal Close), bool?>
    {
        public LongLowerShadowByTuple(IEnumerable<(decimal Open, decimal Low, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25M) 
            : base(inputs, i => i, (i, otm) => otm, periodCount, threshold)
        {
        }
    }

    public class LongLowerShadow : LongLowerShadow<Candle, AnalyzableTick<bool?>>
    {
        public LongLowerShadow(IEnumerable<Candle> inputs, int periodCount = 20, decimal threshold = 0.25M) 
            : base(inputs, i => (i.Open, i.Low, i.Close), (i, otm) => new AnalyzableTick<bool?>(i.DateTime, otm), periodCount, threshold)
        {
        }
    }
}