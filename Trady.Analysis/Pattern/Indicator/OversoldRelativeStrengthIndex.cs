using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class OversoldRelativeStrengthIndex : PatternBase<NonDirectionalPatternResult>
    {
        private RelativeStrengthIndex _rsiIndicator;
        private bool _isUse20;

        public OversoldRelativeStrengthIndex(Equity series, int periodCount, bool isUse20 = false) : base(series)
        {
            _rsiIndicator = new RelativeStrengthIndex(series, periodCount);
            _isUse20 = isUse20;
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            var result = _rsiIndicator.ComputeByIndex(index);
            return new NonDirectionalPatternResult(Series[index].DateTime, _isUse20 ? result.Rsi <= 20 : result.Rsi <= 30);
        }
    }
}
