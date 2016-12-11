using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageCrossover : PatternBase<IsMatchedMultistateResult<Trend>>
    {
        private ExponentialMovingAverageOscillator _emaOscillator;

        public ExponentialMovingAverageCrossover(Equity equity, int periodCount1, int periodCount2) : base(equity)
        {
            _emaOscillator = new ExponentialMovingAverageOscillator(equity, periodCount1, periodCount2);
        }

        protected override TickBase ComputeResultByIndex(int index)
        {
            if (index < 1)
                return new IsMatchedMultistateResult<Trend>(Equity[index].DateTime, false, Trend.NonTrended);

            var latest = _emaOscillator.ComputeByIndex(index);
            var secondLatest = _emaOscillator.ComputeByIndex(index - 1);
            return new IsMatchedMultistateResult<Trend>(Equity[index].DateTime, latest.Osc * secondLatest.Osc < 0, GetTrend(latest.Osc));
        }

        private Trend GetTrend(decimal value)
        {
            if (value > 0) return Trend.Bullish;
            if (value < 0) return Trend.Bearish;
            return Trend.NonTrended;
        }
    }
}
