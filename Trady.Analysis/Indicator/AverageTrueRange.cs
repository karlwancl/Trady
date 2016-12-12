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
        private Ema _trEma;

        public AverageTrueRange(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _tr = i => i > 0 ? new List<decimal?> { Math.Abs(Equity[i].High - Equity[i].Low), Math.Abs(Equity[i].High - Equity[i - 1].Close), Math.Abs(Equity[i].Low - Equity[i - 1].Close) }.Max() : null;

            _trEma = new Ema(
                i => Equity[i].DateTime,
                i => _tr(i),
                i => Enumerable.Range(i - periodCount + 1, periodCount).Average(j => _tr(j)),
                periodCount,
                periodCount,
                true);
        }

        public override IndicatorResult ComputeByIndex(int index)
            => new IndicatorResult(Equity[index].DateTime, _trEma.Compute(index));
    }
}
