using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class LongUpperShadow : AnalyzableBase<(decimal Open, decimal High, decimal Close), bool?>
    {
        public LongUpperShadow(IList<Candle> candles, int periodCount = 20, decimal threshold = 0.75m)
            : this (candles.Select(c => (c.Open, c.High, c.Close)).ToList(), periodCount, threshold)
        {

        }

        public LongUpperShadow(IList<(decimal Open, decimal High, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.75m) : base(inputs)
        {
            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; private set; }
        public decimal Threshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            var upperShadows = Inputs.Select(i => i.High - Math.Max(i.Open, i.Close)).ToList();
            return upperShadows[index] >= upperShadows.Percentile(PeriodCount, index, Threshold);
        }
    }
}