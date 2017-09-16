using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class HistoricalLowestLow : HistoricalLowest<Candle, AnalyzableTick<decimal?>>
    {
        public HistoricalLowestLow(IEnumerable<Candle> inputs)
            : base(inputs, i => i.Low)
        {
        }
    }
}