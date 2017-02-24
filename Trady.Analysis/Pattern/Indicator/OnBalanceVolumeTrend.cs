using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class OnBalanceVolumeTrend : IndicatorBase<PatternResult<Trend?>>
    {
        private OnBalanceVolume _obvIndicator;

        public OnBalanceVolumeTrend(Equity equity) : base(equity)
        {
            _obvIndicator = new OnBalanceVolume(equity);
        }

        protected override PatternResult<Trend?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new PatternResult<Trend?>(Equity[index].DateTime, null);

            var latest = _obvIndicator.ComputeByIndex(index);
            var secondLatest = _obvIndicator.ComputeByIndex(index - 1);

            return new PatternResult<Trend?>(Equity[index].DateTime, Decision.IsTrending(latest.Obv - secondLatest.Obv));
        }
    }
}