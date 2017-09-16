using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsBreakingHistoricalHighestClose : IsBreakingHistoricalHighest<Candle, AnalyzableTick<bool?>>
    {
        public IsBreakingHistoricalHighestClose(IEnumerable<Candle> inputs)
            : base(inputs, i => i.Close)
        {
        }
    }
}
