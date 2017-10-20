using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class RateOfChange : PercentageDifference<Candle, AnalyzableTick<decimal?>>
    {
        public RateOfChange(IEnumerable<Candle> inputs, int numberOfDays = 1)
            : base(inputs, i => i.Close, numberOfDays)
        {
        }
    }
}