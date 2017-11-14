using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class HistoricalHighestClose : HistoricalHighest<IOhlcv, AnalyzableTick<decimal?>>
    {
        public HistoricalHighestClose(IEnumerable<IOhlcv> inputs)
            : base(inputs, i => i.Close)
        {
        }
    }
}