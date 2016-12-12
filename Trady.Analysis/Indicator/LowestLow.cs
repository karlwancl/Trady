using System;
using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.LowestLow;

namespace Trady.Analysis.Indicator
{
    public partial class LowestLow : IndicatorBase<IndicatorResult>
    {
        public LowestLow(Equity equity, int periodCount) : base(equity, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        public override IndicatorResult ComputeByIndex(int index)
        {
            decimal? lowestLow = index >= PeriodCount - 1 ? Equity.Skip(index - PeriodCount + 1).Take(PeriodCount).Min(c => c.Low) : (decimal?)null;
            return new IndicatorResult(Equity[index].DateTime, lowestLow);
        }
    }
}
