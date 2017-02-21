using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageCrossover : IndicatorBase<IsMatchedMultistateResult<Trend?>>
    {
        private ExponentialMovingAverageOscillator _emaOscillator;

        public ExponentialMovingAverageCrossover(Equity equity, int periodCount1, int periodCount2)
            : base(equity, periodCount1, periodCount2)
        {
            _emaOscillator = new ExponentialMovingAverageOscillator(equity, periodCount1, periodCount2);
        }

        protected override IsMatchedMultistateResult<Trend?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new IsMatchedMultistateResult<Trend?>(Equity[index].DateTime, null, null);

            var latest = _emaOscillator.ComputeByIndex(index);
            var secondLatest = _emaOscillator.ComputeByIndex(index - 1);

            return new IsMatchedMultistateResult<Trend?>(Equity[index].DateTime,
                Decision.IsCrossOver(latest.Osc, secondLatest.Osc), Decision.IsTrending(latest.Osc));
        }
    }
}