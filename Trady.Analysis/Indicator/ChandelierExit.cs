using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ChandelierExit : IndicatorBase<(decimal High, decimal Low, decimal Close), (decimal? Long, decimal? Short)>
    {
        private HighestHigh _hh;
        private LowestLow _ll;
        private AverageTrueRange _atr;

        public ChandelierExit(IList<Candle> candles, int periodCount, int atrCount) :
            this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount, atrCount)
        {
        }

        public ChandelierExit(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int atrCount) : base(inputs, periodCount, atrCount)
        {
            _hh = new HighestHigh(inputs.Select(i => i.High).ToList(), periodCount);
            _ll = new LowestLow(inputs.Select(i => i.Low).ToList(), periodCount);
            _atr = new AverageTrueRange(inputs, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public int AtrCount => Parameters[1];

        protected override (decimal? Long, decimal? Short) ComputeByIndexImpl(int index)
        {
            if (index < PeriodCount)
                return (null, null);

            var atr = _atr[index];
            var @long = _hh[index] - atr * AtrCount;
            var @short = _ll[index] + atr * AtrCount;
            return (@long, @short);
        }
    }
}