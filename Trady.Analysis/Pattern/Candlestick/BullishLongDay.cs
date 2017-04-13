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
	public class BullishLongDay : AnalyzableBase<(decimal Open, decimal Close), bool>
	{
        private Bullish _bullish;
        private LongDay _longDay;

		public BullishLongDay(IList<Candle> candles, int periodCount = 20, decimal threshold = 0.75m)
		    : this(candles.Select(c => (c.Open, c.Close)).ToList(), periodCount, threshold)
		{
		}

		public BullishLongDay(IList<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.75m)
		    : base(inputs)
		{
            _bullish = new Bullish(inputs);

			PeriodCount = periodCount;
			Threshold = threshold;
		}

		public int PeriodCount { get; private set; }

		public decimal Threshold { get; private set; }

        protected override bool ComputeByIndexImpl(int index)
            => _bullish[index] && _longDay[index];
	}
}