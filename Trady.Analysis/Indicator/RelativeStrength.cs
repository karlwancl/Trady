using System;
using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.RelativeStrength;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrength : IndicatorBase<IndicatorResult>
    {
        private ClosePriceChange _closePriceChangeIndicator;
        private GenericExponentialMovingAverage _uEma, _dEma;

        public RelativeStrength(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _closePriceChangeIndicator = new ClosePriceChange(equity);

            Func<int, decimal> u = i => Math.Max(_closePriceChangeIndicator.ComputeByIndex(i).Change.Value, 0);
            Func<int, decimal> l = i => Math.Abs(Math.Min(_closePriceChangeIndicator.ComputeByIndex(i).Change.Value, 0));

            _uEma = new GenericExponentialMovingAverage(
                equity,
                periodCount,
                i => i > 0 ? Enumerable.Range(i - PeriodCount + 1, PeriodCount).Average(j => u(j)) : (decimal?)null,
                i => u(i),
                i => 1.0m / periodCount);

            _dEma = new GenericExponentialMovingAverage(
                equity,
                periodCount,
                i => i > 0 ? Enumerable.Range(i - PeriodCount + 1, PeriodCount).Average(j => l(j)) : (decimal?)null,
                i => l(i),
                i => 1.0m / periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            decimal? gain = _uEma.ComputeByIndex(index).Ema;
            decimal? loss = _dEma.ComputeByIndex(index).Ema;
            return new IndicatorResult(Equity[index].DateTime, gain / loss);
        }
    }
}