using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveSimpleMovingAverage : IndicatorBase<PatternResult<Match?>>
    {
        private SimpleMovingAverage _smaIndicator;

        public IsAboveSimpleMovingAverage(Equity equity, int periodCount)
            : base(equity, periodCount)
        {
            _smaIndicator = new SimpleMovingAverage(equity, periodCount);
        }

        protected override PatternResult<Match?> ComputeByIndexImpl(int index)
        {
            var result = _smaIndicator.ComputeByIndex(index);
            return new PatternResult<Match?>(Equity[index].DateTime, Decision.IsMatch(Equity[index].Close.IsLargerThan(result.Sma)));
        }
    }
}