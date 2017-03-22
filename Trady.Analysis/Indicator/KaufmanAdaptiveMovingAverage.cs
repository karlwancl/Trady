using System;
using Trady.Core;
using static Trady.Analysis.Indicator.KaufmanAdaptiveMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class KaufmanAdaptiveMovingAverage : IndicatorBase<IndicatorResult>
    {
        private EfficiencyRatio _erIndicator;
        private GenericExponentialMovingAverage _gemaIndicator;

        public KaufmanAdaptiveMovingAverage(Equity equity, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount)
            : base(equity, periodCount, emaFastPeriodCount, emaSlowPeriodCount)
        {
            _erIndicator = new EfficiencyRatio(equity, periodCount);

            Func<int, decimal> sc = i =>
            {
                double erValue = Convert.ToDouble(_erIndicator.ComputeByIndex(i).Er);
                return Convert.ToDecimal(Math.Pow(erValue * (2.0 / (emaFastPeriodCount + 1) - 2.0 / (emaSlowPeriodCount + 1)) + 2.0 / (emaSlowPeriodCount + 1), 2));
            };

            _gemaIndicator = new GenericExponentialMovingAverage(
                equity,
                periodCount - 1,
                i => Equity[i].Close,
                i => Equity[i].Close,
                i => sc(i));
        }

        protected override IndicatorResult ComputeByIndexImpl(int index)
            => new IndicatorResult(Equity[index].DateTime, _gemaIndicator.ComputeByIndex(index).Ema);
    }
}