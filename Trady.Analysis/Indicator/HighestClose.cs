using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.HighestClose;

namespace Trady.Analysis.Indicator
{
    public partial class HighestClose : IndicatorBase<IndicatorResult>
    {
        public HighestClose(Equity equity, params int[] parameters) : base(equity, parameters)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            decimal? highestClose = index >= PeriodCount - 1 ? Equity.Skip(index - PeriodCount + 1).Take(PeriodCount).Max(c => c.Close) : (decimal?)null;
            return new IndicatorResult(Equity[index].DateTime, highestClose);
        }
    }
}
