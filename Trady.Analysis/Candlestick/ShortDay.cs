using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Extension;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    public class ShortDay<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        public ShortDay(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, int periodCount = 20, decimal threshold = 0.25m) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }

        public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal Close)> mappedInputs, int index)
        {
            var bodyLengths = mappedInputs.Select(i => Math.Abs(i.Close - i.Open));
            return bodyLengths.ElementAt(index) < bodyLengths.Percentile(PeriodCount, Threshold)[index];
        }
    }

    public class ShortDayByTuple : ShortDay<(decimal Open, decimal Close), bool>
    {
        public ShortDayByTuple(IEnumerable<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25M)
            : base(inputs, i => i, periodCount, threshold)
        {
        }
    }

    public class ShortDay : ShortDay<IOhlcv, AnalyzableTick<bool>>
    {
        public ShortDay(IEnumerable<IOhlcv> inputs, int periodCount = 20, decimal threshold = 0.25M)
            : base(inputs, i => (i.Open, i.Close), periodCount, threshold)
        {
        }
    }
}