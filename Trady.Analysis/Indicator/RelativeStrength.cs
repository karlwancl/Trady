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
        private Ema _uEma, _dEma;

        public RelativeStrength(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _closePriceChangeIndicator = new ClosePriceChange(equity);

            _uEma = new Ema(
                i => Equity[i].DateTime,
                i => MathExt.Max(_closePriceChangeIndicator.ComputeByIndex(i).Change, 0).Abs(),
                i => Enumerable
                    .Range(i - PeriodCount + 1, PeriodCount)
                    .Select(j => _closePriceChangeIndicator.ComputeByIndex(j).Change)
                    .Average(v => MathExt.Max(v, 0).Abs()),
                periodCount,
                periodCount,
                true);

            _dEma = new Ema(
                i => Equity[i].DateTime,
                i => MathExt.Min(_closePriceChangeIndicator.ComputeByIndex(i).Change, 0).Abs(),
                i => Enumerable
                    .Range(i - PeriodCount + 1, PeriodCount)
                    .Select(j => _closePriceChangeIndicator.ComputeByIndex(j).Change)
                    .Average(v => MathExt.Min(v, 0).Abs()),
                periodCount,
                periodCount,
                true);
        }

        public int PeriodCount => Parameters[0];

        public override IndicatorResult ComputeByIndex(int index)
        {
            decimal? gain = _uEma.Compute(index);
            decimal? loss = _dEma.Compute(index);
            return new IndicatorResult(Equity[index].DateTime, gain / loss);
        }
    }
}
