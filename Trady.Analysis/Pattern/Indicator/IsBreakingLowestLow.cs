using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsBreakingLowestLow : IsBreakingLowest<Candle, AnalyzableTick<bool?>>
    {
        public IsBreakingLowestLow(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.Low, periodCount)
        {
        }
    }
}
