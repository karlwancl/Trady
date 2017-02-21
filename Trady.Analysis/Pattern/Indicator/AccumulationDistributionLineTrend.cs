using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class AccumulationDistributionLineTrend : IndicatorBase<MultistateResult<Trend?>>
    {
        private AccumulationDistributionLine _accumDistIndicator;

        public AccumulationDistributionLineTrend(Equity series) : base(series)
        {
            _accumDistIndicator = new AccumulationDistributionLine(series);
        }

        protected override MultistateResult<Trend?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new MultistateResult<Trend?>(Equity[index].DateTime, null);

            var latest = _accumDistIndicator.ComputeByIndex(index);
            var secondLatest = _accumDistIndicator.ComputeByIndex(index - 1);

            return new MultistateResult<Trend?>(Equity[index].DateTime,
                Decision.IsTrending(latest.AccumDist - secondLatest.AccumDist));
        }
    }
}