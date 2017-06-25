using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/r/rising-three-methods.asp
    /// </summary>
    public class RisingThreeMethods : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private UpTrend _upTrend;
        private ShortDay _shortDay;
        private BullishLongDay _bullishLongDay;

        public RisingThreeMethods(IList<Candle> candles, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m)
            : this(candles.Select(c => (c.Open, c.High, c.Low, c.Close)).ToList(), upTrendPeriodCount, periodCount, shortThreshold, longThreshold)
        {
        }

        public RisingThreeMethods(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m) : base(inputs)
        {
            var ocs = inputs.Select(i => (i.Open, i.Close)).ToList();

            _upTrend = new UpTrend(inputs.Select(i => (i.High, i.Low)).ToList(), upTrendPeriodCount);
            _shortDay = new ShortDay(ocs, periodCount, shortThreshold);
            _bullishLongDay = new BullishLongDay(ocs, periodCount, longThreshold);

            UpTrendPeriodCount = upTrendPeriodCount;
            PeriodCount = periodCount;
            ShortThreshold = shortThreshold;
            LongThreshold = longThreshold;
        }

        public int UpTrendPeriodCount { get; private set; }
        public int PeriodCount { get; private set; }
        public decimal ShortThreshold { get; private set; }
        public decimal LongThreshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index == 0)
                return null;

            if (!_bullishLongDay[index] || !_shortDay[index - 1])
                return false;

            Func<int, bool> isDesc = i => Inputs[i].Close < Inputs[i - 1].Close && Inputs[i].Open < Inputs[i - 1].Open;
            for (int i = index - 1; i >= UpTrendPeriodCount; i--)
            {
                if (_shortDay[i] && !_bullishLongDay[i - 1] && !isDesc(i))
                    return false;
                else if (_bullishLongDay[i])
                    return (Inputs[index].High > Inputs[i].High) && 
                        Inputs[i].High > Inputs[i+1].High && 
                        Inputs[i].Low < Inputs[index - 1].Low && 
                        (_upTrend[i] ?? false);
            }
            return false;
        }
    }
}