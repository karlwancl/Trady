using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsHighestPrice : IndicatorBase<decimal, Match?>
    {
        public IsHighestPrice(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public IsHighestPrice(IList<decimal> closes, int periodCount)
            : base(closes, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override Match? ComputeByIndexImpl(int index)
            => StateHelper.IsMatch(Inputs.Skip(Inputs.Count - PeriodCount).Max() == Inputs[index]);
    }
}