using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsBreakingHistoricalHighestHigh : IsBreakingHistoricalHighest<Candle, AnalyzableTick<bool?>>
    {
        public IsBreakingHistoricalHighestHigh(IEnumerable<Candle> inputs)
            : base(inputs, i => i.High)
        {
        }
    }
}
