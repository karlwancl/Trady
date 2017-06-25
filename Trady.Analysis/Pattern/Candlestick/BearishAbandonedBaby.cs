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
    public class BearishAbandonedBaby : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private UpTrend _upTrend;
        private BullishLongDay _bullishLongDay;
        private BearishLongDay _bearishLongDay;
        private Doji _doji;

        public BearishAbandonedBaby(IList<Candle> inputs, int upTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75m, decimal dojiThreshold = 0.1m) 
            : this(inputs.Select(i => (i.Open, i.High, i.Low, i.Close)).ToList(), upTrendPeriodCount, longPeriodCount, longThreshold, dojiThreshold)
        {
        }

        public BearishAbandonedBaby(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75m, decimal dojiThreshold = 0.1m) : base(inputs)
        {
            var ocs = inputs.Select(i => (i.Open, i.Close)).ToList();
            _upTrend = new UpTrend(inputs.Select(i => (i.High, i.Low)).ToList(), upTrendPeriodCount);
            _bullishLongDay = new BullishLongDay(ocs, longPeriodCount, longThreshold);
            _doji = new Doji(inputs, dojiThreshold);
            _bearishLongDay = new BearishLongDay(ocs, longPeriodCount, longThreshold);

            UpTrendPeriodCount = upTrendPeriodCount;
            LongPeriodCount = longPeriodCount;
            LongThreshold = longThreshold;
            DojiThreshold = dojiThreshold;
        }

        public int UpTrendPeriodCount { get; private set; }

        public int LongPeriodCount { get; private set; }

        public decimal LongThreshold { get; private set; }

        public decimal DojiThreshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index < 2) return null;
            if (!_doji[index - 1]) return false;
            bool isGapped = Inputs[index - 1].Low > Inputs[index - 2].High && Inputs[index - 1].Low > Inputs[index].High;
            return (_upTrend[index - 1] ?? false) && _bullishLongDay[index - 2] && isGapped && _bearishLongDay[index];
        }
    }
}