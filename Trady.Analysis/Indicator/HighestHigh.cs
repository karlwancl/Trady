using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class HighestHigh : Highest<Candle, AnalyzableTick<decimal?>>
    {
        public HighestHigh(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.High, periodCount)
        {
        }
    }
}