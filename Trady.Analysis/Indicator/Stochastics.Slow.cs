using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class Slow : IndicatorBase<(decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J)>
        {
            private const int SmaPeriodCountK = 3;
            private Full _fullSto;

            public Slow(IList<Candle> candles, int periodCount, int smaPeriodCountD)
                : this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount, smaPeriodCountD)
            {
            }

            public Slow(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountD)
                : base(inputs, periodCount, smaPeriodCountD)
            {
                _fullSto = new Full(inputs, periodCount, SmaPeriodCountK, smaPeriodCountD);
            }

            public int PeriodCount => Parameters[0];

            public int SmaPeriodCountD => Parameters[1];

            protected override (decimal? K, decimal? D, decimal? J) ComputeByIndexImpl(int index) => _fullSto[index];
        }
    }
}