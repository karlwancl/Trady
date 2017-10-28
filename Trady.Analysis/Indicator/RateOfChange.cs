using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class RateOfChange : PercentageDifference<IOhlcvData, AnalyzableTick<decimal?>>
    {
        public RateOfChange(IEnumerable<IOhlcvData> inputs, int numberOfDays = 1)
            : base(inputs, i => i.Close, numberOfDays)
        {
        }
    }
}