using System;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class MovingAverageConvergenceDivergence : IndicatorBase
    {
        private ExponentialMovingAverage _emaIndicator1, _emaIndicator2;
        private Ema _ema;

        public MovingAverageConvergenceDivergence(Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) 
            : base(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
            _emaIndicator1 = new ExponentialMovingAverage(equity, emaPeriodCount1);
            _emaIndicator2 = new ExponentialMovingAverage(equity, emaPeriodCount2);

            _ema = new Ema(
                i => Equity[i].DateTime,
                i => _emaIndicator1.ComputeByIndex(i).Ema - _emaIndicator2.ComputeByIndex(i).Ema, 
                demPeriodCount);
        }

        public int EmaPeriodCount1 => Parameters[0];

        public int EmaPeriodCount2 => Parameters[1];

        public int DemPeriodCount => Parameters[2];

        protected override TickBase ComputeResultByIndex(int index)
        {
            decimal diff = _ema.IndexedFunction(index);
            decimal dem = _ema.Compute(index);
            return new IndicatorResult(Equity[index].DateTime, diff, dem, diff - dem);
        }

        public TimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new TimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
