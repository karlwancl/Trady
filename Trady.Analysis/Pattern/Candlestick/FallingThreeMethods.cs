using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/f/falling-three-methods.asp
    /// </summary>
    public class FallingThreeMethods : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private DownTrend _downTrend;
        private ShortDay _shortDay;
        private BearishLongDay _bearishLongDay;

        public FallingThreeMethods(IList<Candle> candles, int downTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m)
            : this(candles.Select(c => (c.Open, c.High, c.Low, c.Close)).ToList(), downTrendPeriodCount, periodCount, shortThreshold, longThreshold)
        {
        }

        public FallingThreeMethods(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m) : base(inputs)
        {
            var ocs = inputs.Select(i => (i.Open, i.Close)).ToList();

            _downTrend = new DownTrend(inputs.Select(i => (i.High, i.Low)).ToList(), downTrendPeriodCount);
            _shortDay = new ShortDay(ocs, periodCount, shortThreshold);
            _bearishLongDay = new BearishLongDay(ocs, periodCount, longThreshold);

            DownTrendPeriodCount = downTrendPeriodCount;
            PeriodCount = periodCount;
            ShortThreshold = shortThreshold;
            LongThreshold = longThreshold;
        }

        public int DownTrendPeriodCount { get; private set; }
        public int PeriodCount { get; private set; }
        public decimal ShortThreshold { get; private set; }
        public decimal LongThreshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index == 0)
                return null;

            if (!_bearishLongDay[index] || !_shortDay[index - 1])
                return false;

            Func<int, bool> isAsc = i => Inputs[i].Close > Inputs[i - 1].Close && Inputs[i].Open > Inputs[i - 1].Open;
            for (int i = index - 1; i >= DownTrendPeriodCount; i--)
            {
                if (_shortDay[i] && !_bearishLongDay[i - 1] && !isAsc(i))
                    return false;
                else if (_bearishLongDay[i])
                    return (Inputs[index].Low < Inputs[i].Low) && 
                        Inputs[i].Low < Inputs[i + 1].Low && 
                        Inputs[i].High > Inputs[index - 1].High && 
                        (_downTrend[i] ?? false);
            }
            return false;
        }
    }
}