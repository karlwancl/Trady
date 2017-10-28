using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class LowestLow : Lowest<IOhlcvData, AnalyzableTick<decimal?>>
    {
        public LowestLow(IEnumerable<IOhlcvData> inputs, int periodCount)
            : base(inputs, i => i.Low, periodCount)
        {
        }
    }
}