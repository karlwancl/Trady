using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/m/eveningstar.asp
    /// </summary>
    public class EveningStar : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private UpTrend _upTrend;
        private BullishLongDay _bullishLongDay;
        private ShortDay _shortDay;
        private BearishLongDay _bearishLongDay;

        public EveningStar(IList<Candle> inputs, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m, decimal threshold = 0.1m)
            : this(inputs.Select(i => (i.Open, i.High, i.Low, i.Close)).ToList(), upTrendPeriodCount, periodCount, shortThreshold, longThreshold, threshold)
        {
        }

        public EveningStar(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m, decimal threshold = 0.1m) : base(inputs)
        {
            var ocs = inputs.Select(c => (c.Open, c.Close)).ToList();
            _upTrend = new UpTrend(inputs.Select(c => (c.High, c.Low)).ToList(), upTrendPeriodCount);
            _bullishLongDay = new BullishLongDay(ocs, periodCount, longThreshold);
            _shortDay = new ShortDay(ocs, periodCount, shortThreshold);
            _bearishLongDay = new BearishLongDay(ocs, periodCount, longThreshold);

            UpTrendPeriodCount = upTrendPeriodCount;
            PeriodCount = periodCount;
            LongThreshold = longThreshold;
            ShortThreshold = shortThreshold;
            Threshold = threshold;
        }

        public int UpTrendPeriodCount { get; private set; }
        public int PeriodCount { get; private set; }
        public decimal LongThreshold { get; private set; }
        public decimal ShortThreshold { get; private set; }
        public decimal Threshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index < 2) return null;

            Func<int, decimal> midPoint = i => (Inputs[i].Open + Inputs[i].Close) / 2;

            return (_upTrend[index - 1] ?? false) &&
                _bullishLongDay[index - 2] &&
                _shortDay[index - 1] &&
                Inputs[index - 1].Close > Inputs[index - 2].Close &&
                _bearishLongDay[index] &&
                Inputs[index].Open < Math.Min(Inputs[index - 1].Open, Inputs[index - 1].Close) &&
                Math.Abs((Inputs[index].Close - midPoint(index - 2)) / midPoint(index - 2)) < Threshold;
        }
    }
}