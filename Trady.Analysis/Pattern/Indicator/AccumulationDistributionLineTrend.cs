using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class AccumulationDistributionLineTrend : IndicatorBase<PatternResult<Trend?>>
    {
        private AccumulationDistributionLine _accumDistIndicator;

        public AccumulationDistributionLineTrend(Equity series) : base(series)
        {
            _accumDistIndicator = new AccumulationDistributionLine(series);
        }

        protected override PatternResult<Trend?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new PatternResult<Trend?>(Equity[index].DateTime, null);

            var latest = _accumDistIndicator.ComputeByIndex(index);
            var secondLatest = _accumDistIndicator.ComputeByIndex(index - 1);

            return new PatternResult<Trend?>(Equity[index].DateTime, Decision.IsTrending(latest.AccumDist - secondLatest.AccumDist));
        }
    }
}