using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageCrossover : AnalyticBase<IsMatchedMultistateResult<Trend?>>
    {
        private ExponentialMovingAverageOscillator _emaOscillator;

        public ExponentialMovingAverageCrossover(Equity equity, int periodCount1, int periodCount2) : base(equity)
        {
            _emaOscillator = new ExponentialMovingAverageOscillator(equity, periodCount1, periodCount2);
        }

        public override IsMatchedMultistateResult<Trend?> ComputeByIndex(int index)
        {
            if (index < 1)
                return new IsMatchedMultistateResult<Trend?>(Equity[index].DateTime, null, null);

            var latest = _emaOscillator.ComputeByIndex(index);
            var secondLatest = _emaOscillator.ComputeByIndex(index - 1);

            return new IsMatchedMultistateResult<Trend?>(Equity[index].DateTime, 
                ResultExt.IsCrossOver(latest.Osc, secondLatest.Osc), ResultExt.IsTrending(latest.Osc));
        }
    }
}
