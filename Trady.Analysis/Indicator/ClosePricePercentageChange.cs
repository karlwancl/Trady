using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class ClosePricePercentageChange : PercentDiff<Candle, AnalyzableTick<decimal?>>
    {
        public ClosePricePercentageChange(IEnumerable<Candle> inputs, int numberOfDays = 1)
            : base(inputs, i => i.Close, numberOfDays)
        {
        }
    }
}