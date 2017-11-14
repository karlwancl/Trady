using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class LowestLow : Lowest<IOhlcv, AnalyzableTick<decimal?>>
    {
        public LowestLow(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => i.Low, periodCount)
        {
        }
    }
}