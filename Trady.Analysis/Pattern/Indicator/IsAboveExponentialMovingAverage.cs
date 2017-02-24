using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveExponentialMovingAverage : IndicatorBase<PatternResult<Match?>>
    {
        private ExponentialMovingAverage _emaIndicator;

        public IsAboveExponentialMovingAverage(Equity equity, int periodCount)
            : base(equity, periodCount)
        {
            _emaIndicator = new ExponentialMovingAverage(equity, periodCount);
        }

        protected override PatternResult<Match?> ComputeByIndexImpl(int index)
        {
            var result = _emaIndicator.ComputeByIndex(index);
            return new PatternResult<Match?>(Equity[index].DateTime, Decision.IsMatch(Equity[index].Close.IsLargerThan(result.Ema)));
        }
    }
}