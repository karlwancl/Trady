using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsBreakingHighestClose : IsBreakingHighest<Candle, AnalyzableTick<bool?>>
    {
        public IsBreakingHighestClose(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}
