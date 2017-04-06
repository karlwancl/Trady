using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ClosePriceChangeTrend : IndicatorBase<decimal, Trend?>
    {
        private ClosePriceChange _closePriceChange;

        public ClosePriceChangeTrend(IList<Core.Candle> candles)
            : this(candles.Select(c => c.Close).ToList())
        {
        }

        public ClosePriceChangeTrend(IList<decimal> closes) : base(closes)
        {
            _closePriceChange = new ClosePriceChange(closes);
        }

        protected override Trend? ComputeByIndexImpl(int index)
            => StateHelper.IsTrending(_closePriceChange[index]);
    }
}