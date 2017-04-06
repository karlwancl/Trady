using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class RelativeStrengthIndexOvertrade : IndicatorBase<decimal, Overtrade?>
    {
        private RelativeStrengthIndex _rsi;

        public RelativeStrengthIndexOvertrade(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public RelativeStrengthIndexOvertrade(IList<decimal> closes, int periodCount)
            : base(closes, periodCount)
        {
            _rsi = new RelativeStrengthIndex(closes, periodCount);
        }

        protected override Overtrade? ComputeByIndexImpl(int index)
            => StateHelper.IsOvertrade(_rsi[index]);
    }
}