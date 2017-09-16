using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class HistoricalLowestClose : HistoricalLowest<Candle, AnalyzableTick<decimal?>>
    {
        public HistoricalLowestClose(IEnumerable<Candle> inputs)
            : base(inputs, i => i.Close)
        {
        }
    }
}