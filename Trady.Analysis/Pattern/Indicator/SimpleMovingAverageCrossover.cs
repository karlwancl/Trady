using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageCrossover : IndicatorBase<IsMatchedMultistateResult<Trend?>>
    {
        private SimpleMovingAverageOscillator _smaOscillator;

        public SimpleMovingAverageCrossover(Equity equity, int periodCount1, int periodCount2) 
            : base(equity, periodCount1, periodCount2)
        {
            _smaOscillator = new SimpleMovingAverageOscillator(equity, periodCount1, periodCount2);
        }

        public override IsMatchedMultistateResult<Trend?> ComputeByIndex(int index)
        {
            if (index < 1)
                return new IsMatchedMultistateResult<Trend?>(Equity[index].DateTime, null, null);

            var latest = _smaOscillator.ComputeByIndex(index);
            var secondLatest = _smaOscillator.ComputeByIndex(index - 1);

            return new IsMatchedMultistateResult<Trend?>(Equity[index].DateTime, 
                Decision.IsCrossOver(latest.Osc, secondLatest.Osc), Decision.IsTrending(latest.Osc));
        }
    }
}
