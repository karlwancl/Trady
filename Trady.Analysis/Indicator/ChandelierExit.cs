using Trady.Core;
using static Trady.Analysis.Indicator.ChandelierExit;

namespace Trady.Analysis.Indicator
{
    public partial class ChandelierExit : IndicatorBase<IndicatorResult>
    {
        private HighestHigh _highestHigh;
        private LowestLow _lowestLow;
        private AverageTrueRange _atrIndicator;

        public ChandelierExit(Equity equity, int periodCount, int atrCount) : base(equity, periodCount, atrCount)
        {
            _highestHigh = new HighestHigh(equity, periodCount);
            _lowestLow = new LowestLow(equity, periodCount);
            _atrIndicator = new AverageTrueRange(equity, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public int AtrCount => Parameters[1];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            var @long = _highestHigh.ComputeByIndex(index).HighestHigh - _atrIndicator.ComputeByIndex(index).Atr * AtrCount;
            var @short = _lowestLow.ComputeByIndex(index).LowestLow + _atrIndicator.ComputeByIndex(index).Atr * AtrCount;
            return new IndicatorResult(Equity[index].DateTime, @long, @short);
        }
    }
}