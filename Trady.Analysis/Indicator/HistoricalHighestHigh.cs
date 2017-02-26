using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.HistoricalHighestHigh;

namespace Trady.Analysis.Indicator
{
    public partial class HistoricalHighestHigh : IndicatorBase<IndicatorResult>
    {
        public HistoricalHighestHigh(Equity equity) : base(equity)
        {
        }

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            decimal? historicalHighestHigh = Equity.Take(index + 1).Max(c => c.High);
            return new IndicatorResult(Equity[index].DateTime, historicalHighestHigh);
        }
    }
}