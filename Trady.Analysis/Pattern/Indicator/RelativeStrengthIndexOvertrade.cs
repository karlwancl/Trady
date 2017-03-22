using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class RelativeStrengthIndexOvertrade : IndicatorBase<PatternResult<Overtrade?>>
    {
        private RelativeStrengthIndex _rsiIndicator;

        public RelativeStrengthIndexOvertrade(Equity equity, int periodCount)
            : base(equity, periodCount)
        {
            _rsiIndicator = new RelativeStrengthIndex(equity, periodCount);
        }

        protected override PatternResult<Overtrade?> ComputeByIndexImpl(int index)
        {
            var result = _rsiIndicator.ComputeByIndex(index);

            return new PatternResult<Overtrade?>(Equity[index].DateTime, Decision.IsOvertrade(result.Rsi));
        }
    }
}