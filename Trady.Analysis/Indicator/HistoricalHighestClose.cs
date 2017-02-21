using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.HistoricalHighestClose;

namespace Trady.Analysis.Indicator
{
    public partial class HistoricalHighestClose : IndicatorBase<IndicatorResult>
    {
        public HistoricalHighestClose(Equity equity, params int[] parameters) : base(equity, parameters)
        {
        }

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            decimal? historicalHighestClose = Equity.Max(c => c.Close);
            return new IndicatorResult(Equity[index].DateTime, historicalHighestClose);
        }
    }
}
