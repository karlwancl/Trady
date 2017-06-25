using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOvertrade
    {
        public class Fast : IndicatorBase
        {
            public Fast(IList<Core.Candle> candles, int periodCount, int smaPeriodCount)
                : this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount, smaPeriodCount)
            {
            }

            public Fast(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCount)
                : base(inputs, new Stochastics.Fast(inputs, periodCount, smaPeriodCount))
            {
            }
        }
    }
}