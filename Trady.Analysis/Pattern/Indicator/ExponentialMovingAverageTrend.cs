using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageTrend : IndicatorBase<decimal, Trend?>
    {
        private ExponentialMovingAverage _ema;

        public ExponentialMovingAverageTrend(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public ExponentialMovingAverageTrend(IList<decimal> closes, int periodCount)
            : base(closes, periodCount)
        {
            _ema = new ExponentialMovingAverage(closes, periodCount);
        }

        protected override Trend? ComputeByIndexImpl(int index)
            => StateHelper.IsTrending(_ema[index]);
    }
}