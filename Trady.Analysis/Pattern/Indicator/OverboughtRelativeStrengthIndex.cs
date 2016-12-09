using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class OverboughtRelativeStrengthIndex : PatternBase<NonDirectionalPatternResult>
    {
        private RelativeStrengthIndex _rsiIndicator;
        private bool _isUse80;

        public OverboughtRelativeStrengthIndex(Equity series, int periodCount, bool isUse80 = false) : base(series)
        {
            _rsiIndicator = new RelativeStrengthIndex(series, periodCount);
            _isUse80 = isUse80;
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            var result = _rsiIndicator.ComputeByIndex(index);
            return new NonDirectionalPatternResult(Series[index].DateTime, _isUse80 ? result.Rsi >=80 : result.Rsi >= 70);
        }
    }
}
