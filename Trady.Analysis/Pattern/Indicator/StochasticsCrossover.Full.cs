using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public class Full : IndicatorBase
        {
            public Full(IList<Core.Candle> candles, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount, smaPeriodCountK, smaPeriodCountD)
            {
            }

            public Full(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : base(inputs, new Stochastics.Full(inputs, periodCount, smaPeriodCountK, smaPeriodCountD))
            {
            }
        }
    }
}