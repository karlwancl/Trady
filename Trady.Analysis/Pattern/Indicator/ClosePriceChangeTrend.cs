using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ClosePriceChangeTrend : IndicatorBase<MultistateResult<Trend?>>
    {
        private ClosePriceChange _closePriceChangeIndicator;

        public ClosePriceChangeTrend(Equity equity) : base(equity)
        {
            _closePriceChangeIndicator = new ClosePriceChange(equity);
        }

        protected override MultistateResult<Trend?> ComputeByIndexImpl(int index)
        {
            var latest = _closePriceChangeIndicator.ComputeByIndex(index);
            return new MultistateResult<Trend?>(Equity[index].DateTime, Decision.IsTrending(latest.Change));
        }
    }
}