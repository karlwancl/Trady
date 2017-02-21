using System;
using Trady.Core;
using static Trady.Analysis.Indicator.MovingAverageConvergenceDivergence;

namespace Trady.Analysis.Indicator
{
    public partial class MovingAverageConvergenceDivergence : IndicatorBase<IndicatorResult>
    {
        private ExponentialMovingAverage _emaIndicator1, _emaIndicator2;
        private GenericExponentialMovingAverage _dem;
        private Func<int, decimal?> _diff;

        public MovingAverageConvergenceDivergence(Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
            _emaIndicator1 = new ExponentialMovingAverage(equity, emaPeriodCount1);
            _emaIndicator2 = new ExponentialMovingAverage(equity, emaPeriodCount2);
            _diff = i => _emaIndicator1.ComputeByIndex(i).Ema - _emaIndicator2.ComputeByIndex(i).Ema;

            _dem = new GenericExponentialMovingAverage(
                equity,
                0,
                i => _diff(i),
                i => _diff(i),
                demPeriodCount);

            RegisterDependents(_emaIndicator1, _emaIndicator2);
        }

        public int EmaPeriodCount1 => Parameters[0];

        public int EmaPeriodCount2 => Parameters[1];

        public int DemPeriodCount => Parameters[2];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            decimal? diff = _diff(index);
            decimal? dem = _dem.ComputeByIndex(index).Ema;
            return new IndicatorResult(Equity[index].DateTime, diff, dem, diff - dem);
        }
    }
}