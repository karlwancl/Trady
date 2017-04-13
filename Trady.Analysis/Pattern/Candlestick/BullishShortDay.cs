using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class BullishShortDay : AnalyzableBase<(decimal Open, decimal Close), bool>
    {
        private Bullish _bullish;
        private ShortDay _shortDay;

        public BullishShortDay(IList<Candle> candles, int periodCount = 20, decimal threshold = 0.25m)
            : this(candles.Select(c => (c.Open, c.Close)).ToList(), periodCount, threshold)
        {
        }

        public BullishShortDay(IList<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25m)
            : base(inputs)
        {
            _bullish = new Bullish(inputs);
            _shortDay = new ShortDay(inputs);

            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; private set; }

        public decimal Threshold { get; private set; }

        protected override bool ComputeByIndexImpl(int index)
            => _bullish[index] && _shortDay[index];
    }
}