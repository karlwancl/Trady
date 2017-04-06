using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class KaufmanAdaptiveMovingAverage : IndicatorBase<decimal, decimal?>
    {
        private EfficiencyRatio _er;
        private GenericExponentialMovingAverage<decimal> _gema;

        public KaufmanAdaptiveMovingAverage(IList<Candle> candles, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount) :
            this(candles.Select(c => c.Close).ToList(), periodCount, emaFastPeriodCount, emaSlowPeriodCount)
        {
        }

        public KaufmanAdaptiveMovingAverage(IList<decimal> closes, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount)
            : base(closes, periodCount, emaFastPeriodCount, emaSlowPeriodCount)
        {
            _er = new EfficiencyRatio(closes, periodCount);

            Func<int, decimal> sc = i =>
            {
                double erValue = Convert.ToDouble(_er[i]);
                return Convert.ToDecimal(Math.Pow(erValue * (2.0 / (emaFastPeriodCount + 1) - 2.0 / (emaSlowPeriodCount + 1)) + 2.0 / (emaSlowPeriodCount + 1), 2));
            };

            _gema = new GenericExponentialMovingAverage<decimal>(
                closes,
                periodCount - 1,
                i => Inputs[i],
                i => Inputs[i],
                i => sc(i));
        }

        protected override decimal? ComputeByIndexImpl(int index) => _gema[index];
    }
}