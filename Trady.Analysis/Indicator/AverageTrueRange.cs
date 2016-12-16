using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;
using static Trady.Analysis.Indicator.AverageTrueRange;

namespace Trady.Analysis.Indicator
{
    public partial class AverageTrueRange : IndicatorBase<IndicatorResult>
    {
        private readonly Func<int, decimal?> _tr;
        private GenericExponentialMovingAverage _trEma;

        public AverageTrueRange(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _tr = i => i > 0 ? new List<decimal?> { Math.Abs(Equity[i].High - Equity[i].Low), Math.Abs(Equity[i].High - Equity[i - 1].Close), Math.Abs(Equity[i].Low - Equity[i - 1].Close) }.Max() : null;

            _trEma = new GenericExponentialMovingAverage(
                equity,
                periodCount,
                i => Enumerable.Range(i - periodCount + 1, periodCount).Average(j => _tr(j)),
                i => _tr(i),
                periodCount,
                true);
        }

        public override IndicatorResult ComputeByIndex(int index)
            => new IndicatorResult(Equity[index].DateTime, _trEma.ComputeByIndex(index).Ema);
    }
}
