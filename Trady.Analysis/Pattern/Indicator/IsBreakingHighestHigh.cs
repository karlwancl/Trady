using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsBreakingHighestHigh : IsBreakingHighest<Candle, AnalyzableTick<bool?>>
    {
        public IsBreakingHighestHigh(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.High, periodCount)
        {
        }
    }
}
