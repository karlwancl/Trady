using Trady.Core;
using static Trady.Analysis.Indicator.SimpleMovingAverageOscillator;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverageOscillator : IndicatorBase<IndicatorResult>
    {
        private SimpleMovingAverage _smaIndicator1, _smaIndicator2;

        public SimpleMovingAverageOscillator(Equity equity, int periodCount1, int periodCount2)
            : base(equity, periodCount1, periodCount2)
        {
            _smaIndicator1 = new SimpleMovingAverage(equity, periodCount1);
            _smaIndicator2 = new SimpleMovingAverage(equity, periodCount2);
        }

        public int PeriodCount1 => Parameters[0];

        public int PeriodCount2 => Parameters[1];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            var osc = _smaIndicator1.ComputeByIndex(index).Sma - _smaIndicator2.ComputeByIndex(index).Sma;
            return new IndicatorResult(Equity[index].DateTime, osc);
        }
    }
}