using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class RelativeStrengthIndexOvertrade : AnalyzableBase<decimal, Overtrade?>
    {
        private RelativeStrengthIndex _rsi;

        public RelativeStrengthIndexOvertrade(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public RelativeStrengthIndexOvertrade(IList<decimal> closes, int periodCount)
            : base(closes)
        {
            _rsi = new RelativeStrengthIndex(closes, periodCount);
        }

        protected override Overtrade? ComputeByIndexImpl(int index)
            => StateHelper.IsOvertrade(_rsi[index]);
    }
}