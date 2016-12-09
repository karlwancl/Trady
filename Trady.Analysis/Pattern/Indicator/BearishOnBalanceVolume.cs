using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class BearishOnBalanceVolume : PatternBase<NonDirectionalPatternResult>
    {
        private OnBalanceVolume _obvIndicator;

        public BearishOnBalanceVolume(Equity series) : base(series)
        {
            _obvIndicator = new OnBalanceVolume(series);
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            if (index < 1)
                return new NonDirectionalPatternResult(Series[index].DateTime, false);

            var latest = _obvIndicator.ComputeByIndex(index);
            var secondLatest = _obvIndicator.ComputeByIndex(index - 1);
            return new NonDirectionalPatternResult(Series[index].DateTime, latest.Obv < secondLatest.Obv);
        }
    }
}
