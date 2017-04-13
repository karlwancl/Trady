using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class BearishShortDay : AnalyzableBase<(decimal Open, decimal Close), bool>
    {
        private Bearish _bearish;
        private ShortDay _shortDay;

        public BearishShortDay(IList<Candle> candles, int periodCount = 20, decimal threshold = 0.25m)
            : this(candles.Select(c => (c.Open, c.Close)).ToList(), periodCount, threshold)
        {
        }

        public BearishShortDay(IList<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25m)
            : base(inputs)
        {
            _bearish = new Bearish(inputs);
            _shortDay = new ShortDay(inputs);

            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; private set; }

        public decimal Threshold { get; private set; }

        protected override bool ComputeByIndexImpl(int index)
            => _bearish[index] && _shortDay[index];
    }
}
