using System;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;
using static Trady.Analysis.Indicator.MovingAverageConvergenceDivergence;

namespace Trady.Analysis.Indicator
{
    public partial class MovingAverageConvergenceDivergence : IndicatorBase<IndicatorResult>
    {
        private ExponentialMovingAverage _emaIndicator1, _emaIndicator2;
        private Ema _dem;

        public MovingAverageConvergenceDivergence(Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) 
            : base(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
            _emaIndicator1 = new ExponentialMovingAverage(equity, emaPeriodCount1);
            _emaIndicator2 = new ExponentialMovingAverage(equity, emaPeriodCount2);

            _dem = new Ema(
                i => Equity[i].DateTime,
                i => _emaIndicator1.ComputeByIndex(i).Ema - _emaIndicator2.ComputeByIndex(i).Ema,
                i => _emaIndicator1.ComputeByIndex(i).Ema - _emaIndicator2.ComputeByIndex(i).Ema,
                demPeriodCount,
                0);
        }

        public int EmaPeriodCount1 => Parameters[0];

        public int EmaPeriodCount2 => Parameters[1];

        public int DemPeriodCount => Parameters[2];

        public override IndicatorResult ComputeByIndex(int index)
        {
            decimal? diff = _dem.ValueFunction(index);
            decimal? dem = _dem.Compute(index);
            return new IndicatorResult(Equity[index].DateTime, diff, dem, diff - dem);
        }
    }
}
