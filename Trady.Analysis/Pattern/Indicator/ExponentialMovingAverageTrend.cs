using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageTrend : IndicatorBase<PatternResult<Trend?>>
    {
        private ExponentialMovingAverage _emaIndicator;

        public ExponentialMovingAverageTrend(Equity equity, int periodCount)
            : base(equity, periodCount)
        {
            _emaIndicator = new ExponentialMovingAverage(equity, periodCount);
        }

        protected override PatternResult<Trend?> ComputeByIndexImpl(int index)
        {
            var result = _emaIndicator.ComputeByIndex(index);
            return new PatternResult<Trend?>(Equity[index].DateTime, Decision.IsTrending(result.Ema));
        }
    }
}