using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class HistoricalHighestHigh : HistoricalHighest<Candle, AnalyzableTick<decimal?>>
    {
        public HistoricalHighestHigh(IEnumerable<Candle> inputs)
            : base(inputs, i => i.High)
        {
        }
    }
}