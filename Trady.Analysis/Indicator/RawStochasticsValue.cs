using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RawStochasticsValue : AnalyzableBase<(decimal High, decimal Low, decimal Close), decimal?>
    {
        private HighestHigh _hh;
        private LowestLow _ll;

        public RawStochasticsValue(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount)
        {
        }

        public RawStochasticsValue(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount) : base(inputs)
        {
            _hh = new HighestHigh(inputs.Select(i => i.High).ToList(), periodCount);
            _ll = new LowestLow(inputs.Select(i => i.Low).ToList(), periodCount);

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index)
        {
            var hh = _hh[index];
            var ll = _ll[index];
            return (hh == ll) ? 50 : 100 * (Inputs[index].Close - ll) / (hh - ll);
        }
    }
}