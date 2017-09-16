using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsBreakingHistoricalLowestLow : IsBreakingHistoricalLowest<Candle, AnalyzableTick<bool?>>
    {
        public IsBreakingHistoricalLowestLow(IEnumerable<Candle> inputs)
            : base(inputs, i => i.Low)
        {
        }
    }
}
