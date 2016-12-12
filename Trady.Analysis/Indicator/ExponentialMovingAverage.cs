using System;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;
using static Trady.Analysis.Indicator.ExponentialMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverage : IndicatorBase<IndicatorResult>
    {
        private Ema _ema;

        public ExponentialMovingAverage(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _ema = new Ema(
                i => Equity[i].DateTime, 
                i => Equity[i].Close, 
                i => Equity[i].Close, 
                periodCount,
                0);
        }

        public int PeriodCount => Parameters[0];

        public override IndicatorResult ComputeByIndex(int index)
            => new IndicatorResult(Equity[index].DateTime, _ema.Compute(index));
    }
}
