using System.Collections.Generic;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class UpMomentum : PositiveDifference<IOhlcv, AnalyzableTick<decimal?>>
    {
        public UpMomentum(IEnumerable<IOhlcv> inputs, int periodCount = 1)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}
