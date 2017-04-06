using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageTrend : IndicatorBase<decimal, Trend?>
    {
        private SimpleMovingAverage _sma;

        public SimpleMovingAverageTrend(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public SimpleMovingAverageTrend(IList<decimal> closes, int periodCount)
            : base(closes, periodCount)
        {
            _sma = new SimpleMovingAverage(closes, periodCount);
        }

        protected override Trend? ComputeByIndexImpl(int index)
            => index >= 1 ? StateHelper.IsTrending(_sma[index] - _sma[index - 1]) : null;
    }
}