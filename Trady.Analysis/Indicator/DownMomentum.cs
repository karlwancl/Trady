using System.Collections.Generic;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class DownMomentum : NegativeDifference<IOhlcvData, AnalyzableTick<decimal?>>
    {
        public DownMomentum(IEnumerable<IOhlcvData> inputs, int periodCount = 1)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}
