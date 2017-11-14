using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Extension;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    public class LongUpperShadow<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Close), bool?, TOutput>
    {
        public LongUpperShadow(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Close)> inputMapper, int periodCount = 20, decimal threshold = 0.75m) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }
        public decimal Threshold { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Close)> mappedInputs, int index)
        {
            var upperShadows = mappedInputs.Select(i => i.High - Math.Max(i.Open, i.Close));
            return upperShadows.ElementAt(index) >= upperShadows.Percentile(PeriodCount, Threshold)[index];
        }
    }

    public class LongUpperShadowByTuple : LongUpperShadow<(decimal Open, decimal High, decimal Close), bool?>
    {
        public LongUpperShadowByTuple(IEnumerable<(decimal Open, decimal High, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.75M)
            : base(inputs, i => i, periodCount, threshold)
        {
        }
    }

    public class LongUpperShadow : LongUpperShadow<IOhlcv, AnalyzableTick<bool?>>
    {
        public LongUpperShadow(IEnumerable<IOhlcv> inputs, int periodCount = 20, decimal threshold = 0.75M)
            : base(inputs, i => (i.Open, i.High, i.Close), periodCount, threshold)
        {
        }
    }
}