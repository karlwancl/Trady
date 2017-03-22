using Trady.Core;
using static Trady.Analysis.Indicator.ModifiedExponentialMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class ModifiedExponentialMovingAverage : IndicatorBase<IndicatorResult>
    {
        private GenericExponentialMovingAverage _gemaIndicator;

        public ModifiedExponentialMovingAverage(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _gemaIndicator = new GenericExponentialMovingAverage(
                equity,
                0,
                i => Equity[i].Close,
                i => Equity[i].Close,
                i => 1.0m / periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override IndicatorResult ComputeByIndexImpl(int index)
            => new IndicatorResult(Equity[index].DateTime, _gemaIndicator.ComputeByIndex(index).Ema);
    }
}