using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageTrend : IndicatorBase<MultistateResult<Trend?>>
    {
        private ExponentialMovingAverage _emaIndicator;

        public ExponentialMovingAverageTrend(Equity equity, int periodCount)
            : base(equity, periodCount)
        {
            _emaIndicator = new ExponentialMovingAverage(equity, periodCount);
        }

        protected override MultistateResult<Trend?> ComputeByIndexImpl(int index)
        {
            var result = _emaIndicator.ComputeByIndex(index);
            return new MultistateResult<Trend?>(Equity[index].DateTime, Decision.IsTrending(result.Ema));
        }
    }
}