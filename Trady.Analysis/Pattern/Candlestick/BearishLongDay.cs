using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class BearishLongDay : AnalyzableBase<(decimal Open, decimal Close), bool>
    {
        private Bearish _bearish;
        private LongDay _longDay;

        public BearishLongDay(IList<Candle> candles, int periodCount = 20, decimal threshold = 0.75m)
		    : this(candles.Select(c => (c.Open, c.Close)).ToList(), periodCount, threshold)
		{
        }

        public BearishLongDay(IList<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.75m)
		    : base(inputs)
		{
            _bearish = new Bearish(inputs);
            _longDay = new LongDay(inputs);

            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; private set; }

        public decimal Threshold { get; private set; }

        protected override bool ComputeByIndexImpl(int index)
            => _bearish[index] && _longDay[index];
    }
}
