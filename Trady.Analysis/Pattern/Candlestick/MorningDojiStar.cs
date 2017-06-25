using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class MorningDojiStar : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private DownTrend _downTrend;
        private BullishLongDay _bullishLongDay;
        private Doji _doji;
        private BearishLongDay _bearishLongDay;

        public MorningDojiStar(IList<Candle> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal longThreshold = 0.75m, decimal dojiThreshold = 0.25m, decimal threshold = 0.1m)
            : this(inputs.Select(i => (i.Open, i.High, i.Low, i.Close)).ToList(), downTrendPeriodCount, periodCount, longThreshold, dojiThreshold, threshold)
        {
        }

        public MorningDojiStar(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal longThreshold = 0.75m, decimal dojiThreshold = 0.25m, decimal threshold = 0.1m) : base(inputs)
        {
            var ocs = inputs.Select(c => (c.Open, c.Close)).ToList();
            _downTrend = new DownTrend(inputs.Select(c => (c.High, c.Low)).ToList(), downTrendPeriodCount);
            _bullishLongDay = new BullishLongDay(ocs, periodCount, longThreshold);
            _doji = new Doji(inputs, dojiThreshold);
            _bearishLongDay = new BearishLongDay(ocs, periodCount, longThreshold);

            UpTrendPeriodCount = downTrendPeriodCount;
            PeriodCount = periodCount;
            LongThreshold = longThreshold;
            DojiThreshold = dojiThreshold;
            Threshold = threshold;
        }

        public int UpTrendPeriodCount { get; private set; }
        public int PeriodCount { get; private set; }
        public decimal LongThreshold { get; private set; }
        public decimal DojiThreshold { get; private set; }
        public decimal Threshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index < 2) return null;

            Func<int, decimal> midPoint = i => (Inputs[i].Open + Inputs[i].Close) / 2;

            return (_downTrend[index - 1] ?? false) &&
                _bearishLongDay[index - 2] &&
                _doji[index - 1] &&
                midPoint(index - 1) < Inputs[index - 2].Close &&
                _bullishLongDay[index] &&
                Inputs[index].Open > Math.Max(Inputs[index - 1].Open, Inputs[index - 1].Close) &&
                Math.Abs((Inputs[index].Close - midPoint(index - 2)) / midPoint(index - 2)) < Threshold;
        }
    }
}