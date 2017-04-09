using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class Fast : AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J)>
        {
            private RawStochasticsValue _rsv;

            public Fast(IList<Candle> candles, int periodCount, int smaPeriodCount)
                : this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount, smaPeriodCount)
            {
            }

            public Fast(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCount)
                : base(inputs)
            {
                _rsv = new RawStochasticsValue(inputs, periodCount);

                PeriodCount = periodCount;
                SmaPeriodCount = smaPeriodCount;
            }

            public int PeriodCount { get; private set; }

            public int SmaPeriodCount { get; private set; }

            protected override (decimal? K, decimal? D, decimal? J) ComputeByIndexImpl(int index)
            {
                decimal? rsv = _rsv[index];
                Func<int, decimal?> rsvFunc = i => _rsv[index - SmaPeriodCount + i + 1];
                decimal? rsvAvg = index >= SmaPeriodCount - 1 ? Enumerable.Range(0, SmaPeriodCount).Average(i => rsvFunc(i)) : null;
                return (rsv, rsvAvg, 3 * rsv - 2 * rsvAvg);
            }
        }
    }
}