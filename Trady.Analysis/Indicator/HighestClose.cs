using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class HighestClose : Highest<IOhlcv, AnalyzableTick<decimal?>>
    {
        public HighestClose(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}