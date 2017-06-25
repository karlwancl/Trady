using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class LongLowerShadow : AnalyzableBase<(decimal Open, decimal Low, decimal Close), bool?>
    {
        public LongLowerShadow(IList<Candle> candles, int periodCount = 20, decimal threshold = 0.25m)
            : this (candles.Select(c => (c.Open, c.Low, c.Close)).ToList(), periodCount, threshold)
        {

        }

        public LongLowerShadow(IList<(decimal Open, decimal Low, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25m) : base(inputs)
        {
            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; private set; }
        public decimal Threshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            var lowerShadows = Inputs.Select(i => Math.Min(i.Open, i.Close) - i.Low).ToList();
            return lowerShadows[index] < lowerShadows.Percentile(PeriodCount, index, Threshold);
        }
    }
}