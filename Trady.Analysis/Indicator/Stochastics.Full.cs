using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class Full : IndicatorBase<(decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J)>
        {
            private Fast _fastSto;

            public Full(IList<Candle> candles, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount, smaPeriodCountK, smaPeriodCountD)
            {
            }

            public Full(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : base(inputs, periodCount, smaPeriodCountK, smaPeriodCountD)
            {
                _fastSto = new Fast(inputs, periodCount, smaPeriodCountK);
            }

            public int PeriodCount => Parameters[0];

            public int SmaPeriodCountK => Parameters[1];

            public int SmaPeriodCountD => Parameters[2];

            protected override (decimal? K, decimal? D, decimal? J) ComputeByIndexImpl(int index)
            {
                var d = _fastSto[index].D;
                Func<int, decimal?> dFunc = i => _fastSto[index - SmaPeriodCountD + i + 1].D;
                decimal? dAvg = index >= SmaPeriodCountK - 1 ? Enumerable.Range(0, SmaPeriodCountD).Average(i => dFunc(i)) : null;
                return (d, dAvg, 3 * d - 2 * dAvg);
            }
        }
    }
}