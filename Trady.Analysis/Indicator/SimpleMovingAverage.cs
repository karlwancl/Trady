using System;
using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.SimpleMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverage : IndicatorBase<IndicatorResult>
    {
        public SimpleMovingAverage(Equity equity, int periodCount) : base(equity, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        public override IndicatorResult ComputeByIndex(int index)
        {
            decimal? sma = index >= PeriodCount - 1 ? Equity.Skip(index - PeriodCount + 1).Take(PeriodCount).Average(c => c.Close) : (decimal?)null;
            return new IndicatorResult(Equity[index].DateTime, sma);
        }
    }
}
