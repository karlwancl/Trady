using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class ChandelierExit : AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal? Long, decimal? Short)>
    {
        private HighestHigh _hh;
        private LowestLow _ll;
        private AverageTrueRange _atr;

        public ChandelierExit(IList<Candle> candles, int periodCount, decimal atrCount) :
            this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount, atrCount)
        {
        }

        public ChandelierExit(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, decimal atrCount) : base(inputs)
        {
            _hh = new HighestHigh(inputs.Select(i => i.High).ToList(), periodCount);
            _ll = new LowestLow(inputs.Select(i => i.Low).ToList(), periodCount);
            _atr = new AverageTrueRange(inputs, periodCount);

            PeriodCount = periodCount;
            AtrCount = atrCount;
        }

        public int PeriodCount { get; private set; }

        public decimal AtrCount { get; private set; }

        protected override (decimal? Long, decimal? Short) ComputeByIndexImpl(int index)
        {
            var atr = _atr[index];
            var @long = _hh[index] - atr * AtrCount;
            var @short = _ll[index] + atr * AtrCount;
            return (@long, @short);
        }
    }
}