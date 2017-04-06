using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrengthIndex : IndicatorBase<decimal, decimal?>
    {
        private RelativeStrength _rs;

        public RelativeStrengthIndex(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public RelativeStrengthIndex(IList<decimal> closes, int periodCount) : base(closes, periodCount)
        {
            _rs = new RelativeStrength(closes, periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override decimal? ComputeByIndexImpl(int index) => 100 - (100 / (1 + _rs[index]));
    }
}