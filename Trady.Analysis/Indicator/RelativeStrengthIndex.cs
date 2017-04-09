using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrengthIndex : AnalyzableBase<decimal, decimal?>
    {
        private RelativeStrength _rs;

        public RelativeStrengthIndex(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public RelativeStrengthIndex(IList<decimal> closes, int periodCount) : base(closes)
        {
            _rs = new RelativeStrength(closes, periodCount);

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index) => 100 - (100 / (1 + _rs[index]));
    }
}