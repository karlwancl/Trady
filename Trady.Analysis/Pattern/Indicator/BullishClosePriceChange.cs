using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class BullishClosePriceChange : PatternBase<NonDirectionalPatternResult>
    {
        private ClosePriceChange _closePriceChangeIndicator;

        public BullishClosePriceChange(Equity series) : base(series)
        {
            _closePriceChangeIndicator = new ClosePriceChange(series);
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            var latest = _closePriceChangeIndicator.ComputeByIndex(index);
            return new NonDirectionalPatternResult(Series[index].DateTime, latest.Change > 0);
        }
    }
}
