using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_bearish_reversal_patterns#bearish_abandoned_baby
    /// </summary>
    public class BullishAbandonedBaby : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private DownTrend _downTrend;
        private BearishLongDay _bearishLongDay;
        private BullishLongDay _bullishLongDay;
        private Doji _doji;

        public BullishAbandonedBaby(IList<Candle> inputs, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75m, decimal dojiThreshold = 0.1m) 
            : this(inputs.Select(i => (i.Open, i.High, i.Low, i.Close)).ToList(), downTrendPeriodCount, longPeriodCount, longThreshold, dojiThreshold)
        {
        }

        public BullishAbandonedBaby(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75m, decimal dojiThreshold = 0.1m) : base(inputs)
        {
            var ocs = inputs.Select(i => (i.Open, i.Close)).ToList();
            _downTrend = new DownTrend(inputs.Select(i => (i.High, i.Low)).ToList(), downTrendPeriodCount);
            _bearishLongDay = new BearishLongDay(ocs, longPeriodCount, longThreshold);
            _bullishLongDay = new BullishLongDay(ocs, longPeriodCount, longThreshold);
            _doji = new Doji(inputs, dojiThreshold);

            DownTrendPeriodCount = downTrendPeriodCount;
            LongPeriodCount = longPeriodCount;
            LongThreshold = longThreshold;
            DojiThreshold = dojiThreshold;
        }

        public int DownTrendPeriodCount { get; private set; }

        public int LongPeriodCount { get; private set; }

        public decimal LongThreshold { get; private set; }

        public decimal DojiThreshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index < 2) return null;
            if (!_doji[index - 1]) return false;
            bool isGapped = Inputs[index - 1].High < Inputs[index - 2].Low && Inputs[index - 1].High < Inputs[index].Low;
            return (_downTrend[index - 1] ?? false) && _bearishLongDay[index - 2] && isGapped && _bullishLongDay[index];
        }
    }
}