using Trady.Core;
using static Trady.Analysis.Indicator.ClosePricePercentageChange;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePricePercentageChange : IndicatorBase<IndicatorResult>
    {
        public ClosePricePercentageChange(Equity equity) : base(equity, null)
        {
        }

        protected override IndicatorResult ComputeByIndexImpl(int index)
           => new IndicatorResult(Equity[index].DateTime, index > 0 ? (Equity[index].Close - Equity[index - 1].Close) / Equity[index - 1].Close : (decimal?)null);
    }
}