using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class RateOfChange : PercentageDifference<IOhlcv, AnalyzableTick<decimal?>>
    {
        public RateOfChange(IEnumerable<IOhlcv> inputs, int periodCount = 1)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}