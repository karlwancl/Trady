using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class LongDay : AnalyzableBase<(decimal Open, decimal Close), bool>
    {
        public LongDay(IList<Candle> candles, int periodCount = 20, decimal threshold = 0.75m)
            : this(candles.Select(c => (c.Open, c.Close)).ToList(), periodCount, threshold)
        {
        }

        public LongDay(IList<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.75m)
            : base(inputs)
        {
            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; private set; }

        public decimal Threshold { get; private set; }

        protected override bool ComputeByIndexImpl(int index)
        {
            var bodyLengths = Inputs.Select(i => Math.Abs(i.Close - i.Open)).ToList();
            return bodyLengths[index] >= bodyLengths.Percentile(PeriodCount, index, Threshold);
        }
    }
}
