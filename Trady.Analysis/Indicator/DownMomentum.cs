using System.Collections.Generic;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class DownMomentum : NegativeDifference<IOhlcv, AnalyzableTick<decimal?>>
    {
        public DownMomentum(IEnumerable<IOhlcv> inputs, int periodCount = 1)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}
