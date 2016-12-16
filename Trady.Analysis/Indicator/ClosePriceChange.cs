using System;
using Trady.Core;
using static Trady.Analysis.Indicator.ClosePriceChange;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePriceChange : IndicatorBase<IndicatorResult>
    {
        public ClosePriceChange(Equity equity) : base(equity)
        {
        }

        public override IndicatorResult ComputeByIndex(int index)
           => new IndicatorResult(Equity[index].DateTime, index > 0 ? Equity[index].Close - Equity[index - 1].Close : (decimal?)null);
    }
}
