using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageCrossover : PatternBase<DirectionalPatternResult>
    {
        private SimpleMovingAverageOscillator _smaOscillator;

        public SimpleMovingAverageCrossover(Equity series, int periodCount1, int periodCount2) : base(series)
        {
            _smaOscillator = new SimpleMovingAverageOscillator(series, periodCount1, periodCount2);
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            if (index < 1)
                return new DirectionalPatternResult(Series[index].DateTime, false, false, false);

            var latest = _smaOscillator.ComputeByIndex(index);
            var secondLatest = _smaOscillator.ComputeByIndex(index - 1);
            return new DirectionalPatternResult(Series[index].DateTime, latest.Osc * secondLatest.Osc < 0, latest.Osc > 0, latest.Osc < 0);
        }
    }
}
