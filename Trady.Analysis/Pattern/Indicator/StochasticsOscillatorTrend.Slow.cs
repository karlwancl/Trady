using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOscillatorTrend
    {
        public class Slow : IndicatorBase
        {
            public Slow(IList<Core.Candle> candles, int periodCount, int smaPeriodCountD)
                : this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount, smaPeriodCountD)
            {
            }

            public Slow(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountD)
                : base(inputs, new Stochastics.Slow(inputs, periodCount, smaPeriodCountD))
            {
            }
        }
    }
}