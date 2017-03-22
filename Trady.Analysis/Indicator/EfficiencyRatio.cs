using System;
using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.EfficiencyRatio;

namespace Trady.Analysis.Indicator
{
    public partial class EfficiencyRatio : IndicatorBase<IndicatorResult>
    {
        public EfficiencyRatio(Equity equity, int periodCount) : base(equity, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            if (index < PeriodCount || index < 1)
                return new IndicatorResult(Equity[index].DateTime, null);

            decimal? change = Math.Abs(Equity[index].Close - Equity[index - PeriodCount].Close);
            decimal? volatility = Enumerable.Range(index - PeriodCount + 1, PeriodCount).Select(i => Math.Abs(Equity[i].Close - Equity[i - 1].Close)).Sum();
            return new IndicatorResult(Equity[index].DateTime, change / volatility);
        }
    }
}