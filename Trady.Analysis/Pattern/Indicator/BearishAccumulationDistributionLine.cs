using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class BearishAccumulationDistributionLine : PatternBase<NonDirectionalPatternResult>
    {
        private AccumulationDistributionLine _accumDistIndicator;

        public BearishAccumulationDistributionLine(Equity series) : base(series)
        {
            _accumDistIndicator = new AccumulationDistributionLine(series);
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            if (index < 1)
                return new NonDirectionalPatternResult(Series[index].DateTime, false);

            var latest = _accumDistIndicator.ComputeByIndex(index);
            var secondLatest = _accumDistIndicator.ComputeByIndex(index - 1);
            return new NonDirectionalPatternResult(Series[index].DateTime, latest.AccumDist < secondLatest.AccumDist);
        }
    }
}
