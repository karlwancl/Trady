using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.HighestHigh;

namespace Trady.Analysis.Indicator
{
    public partial class HighestHigh : IndicatorBase<IndicatorResult>
    {
        public HighestHigh(Equity equity, params int[] parameters) : base(equity, parameters)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            decimal? highestHigh = index >= PeriodCount - 1 ? Equity.Skip(index - PeriodCount + 1).Take(PeriodCount).Max(c => c.High) : (decimal?)null;
            return new IndicatorResult(Equity[index].DateTime, highestHigh);
        }
    }
}