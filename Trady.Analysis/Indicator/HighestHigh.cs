using System;
using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.HighestHigh;

namespace Trady.Analysis.Indicator
{
    public partial class HighestHigh : IndicatorBase<IndicatorResult>
    {
        public HighestHigh(Equity equity, int periodCount) : base(equity, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        public override IndicatorResult ComputeByIndex(int index)
        {
            decimal? highestHigh = index >= PeriodCount - 1 ? Equity.Skip(index - PeriodCount + 1).Take(PeriodCount).Max(c => c.High) : (decimal?)null;
            return new IndicatorResult(Equity[index].DateTime, highestHigh);
        }
    }
}
