using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class BearishClosePriceChange : PatternBase<NonDirectionalPatternResult>
    {
        private ClosePriceChange _closePriceChangeIndicator;

        public BearishClosePriceChange(Equity series) : base(series)
        {
            _closePriceChangeIndicator = new ClosePriceChange(series);
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            var latest = _closePriceChangeIndicator.ComputeByIndex(index);
            return new NonDirectionalPatternResult(Series[index].DateTime, latest.Change < 0);
        }
    }
}
