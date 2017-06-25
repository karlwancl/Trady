using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class EveningDojiStar : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private UpTrend _upTrend;
        private BullishLongDay _bullishLongDay;
        private Doji _doji;
        private BearishLongDay _bearishLongDay;

        public EveningDojiStar(IList<Candle> inputs, int upTrendPeriodCount = 3, int periodCount = 20,  decimal longThreshold = 0.75m, decimal dojiThreshold = 0.25m, decimal threshold = 0.1m)
            : this(inputs.Select(i => (i.Open, i.High, i.Low, i.Close)).ToList(), upTrendPeriodCount, periodCount, longThreshold, dojiThreshold, threshold)
        {
        }

        public EveningDojiStar(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, int periodCount = 20, decimal longThreshold = 0.75m, decimal dojiThreshold = 0.25m, decimal threshold = 0.1m) : base(inputs)
        {
            var ocs = inputs.Select(c => (c.Open, c.Close)).ToList();
            _upTrend = new UpTrend(inputs.Select(c => (c.High, c.Low)).ToList(), upTrendPeriodCount);
            _bullishLongDay = new BullishLongDay(ocs, periodCount, longThreshold);
            _doji = new Doji(inputs, dojiThreshold);
            _bearishLongDay = new BearishLongDay(ocs, periodCount, longThreshold);

            UpTrendPeriodCount = upTrendPeriodCount;
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

            return (_upTrend[index - 1] ?? false) &&
                _bullishLongDay[index - 2] &&
                _doji[index - 1] &&
                midPoint(index - 1) > Inputs[index - 2].Close &&
                _bearishLongDay[index] &&
                Inputs[index].Open < Math.Min(Inputs[index - 1].Open, Inputs[index - 1].Close) &&
                Math.Abs((Inputs[index].Close - midPoint(index - 2)) / midPoint(index - 2)) < Threshold;
        }
    }
}