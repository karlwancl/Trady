using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageTrend : IndicatorBase<MultistateResult<Trend?>>
    {
        private SimpleMovingAverage _smaIndicator;

        public SimpleMovingAverageTrend(Equity equity, int periodCount)
            : base(equity, periodCount)
        {
            _smaIndicator = new SimpleMovingAverage(equity, periodCount);
        }

        protected override MultistateResult<Trend?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new MultistateResult<Trend?>(Equity[index].DateTime, null);

            var currentValue = _smaIndicator.ComputeByIndex(index).Sma;
            var previousValue = _smaIndicator.ComputeByIndex(index - 1).Sma;

            return new MultistateResult<Trend?>(Equity[index].DateTime,
                Decision.IsTrending(currentValue - previousValue));
        }
    }
}