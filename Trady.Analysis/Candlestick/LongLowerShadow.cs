using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Extension;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

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
            return lowerShadows.ElementAt(index) < lowerShadows.Percentile(PeriodCount, Threshold)[index];
        }
    }

    public class LongLowerShadowByTuple : LongLowerShadow<(decimal Open, decimal Low, decimal Close), bool?>
    {
        public LongLowerShadowByTuple(IEnumerable<(decimal Open, decimal Low, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25M)
            : base(inputs, i => i, periodCount, threshold)
        {
        }
    }

    public class LongLowerShadow : LongLowerShadow<IOhlcv, AnalyzableTick<bool?>>
    {
        public LongLowerShadow(IEnumerable<IOhlcv> inputs, int periodCount = 20, decimal threshold = 0.25M)
            : base(inputs, i => (i.Open, i.Low, i.Close), periodCount, threshold)
        {
        }
    }
}