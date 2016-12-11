using System;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverage : IndicatorBase
    {
        private Ema _ema;

        public ExponentialMovingAverage(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _ema = new Ema(i => Equity[i].DateTime, i => Equity[i].Close, periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override TickBase ComputeResultByIndex(int index)
            => new IndicatorResult(Equity[index].DateTime, _ema.Compute(index));

        public TimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new TimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
