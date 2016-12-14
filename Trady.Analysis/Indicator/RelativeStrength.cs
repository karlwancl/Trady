using System;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
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

            _uEma = new GenericExponentialMovingAverage(
                equity,
                periodCount,
                i => Enumerable
                    .Range(i - PeriodCount + 1, PeriodCount)
                    .Select(j => _closePriceChangeIndicator.ComputeByIndex(j).Change)
                    .Average(v => Math2.Max(v, 0).Abs()),
                i => Math2.Max(_closePriceChangeIndicator.ComputeByIndex(i).Change, 0).Abs(),
                periodCount,
                true);

            _dEma = new GenericExponentialMovingAverage(
                equity,
                periodCount,
                i => Enumerable
                    .Range(i - PeriodCount + 1, PeriodCount)
                    .Select(j => _closePriceChangeIndicator.ComputeByIndex(j).Change)
                    .Average(v => Math2.Min(v, 0).Abs()),
                i => Math2.Min(_closePriceChangeIndicator.ComputeByIndex(i).Change, 0).Abs(),
                periodCount,
                true);
        }

        public int PeriodCount => Parameters[0];

        public override IndicatorResult ComputeByIndex(int index)
        {
            decimal? gain = _uEma.ComputeByIndex(index).Ema;
            decimal? loss = _dEma.ComputeByIndex(index).Ema;
            return new IndicatorResult(Equity[index].DateTime, gain / loss);
        }
    }
}
