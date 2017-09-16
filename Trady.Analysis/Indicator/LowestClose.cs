using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class LowestClose : Lowest<Candle, AnalyzableTick<decimal?>>
    {
        public LowestClose(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}