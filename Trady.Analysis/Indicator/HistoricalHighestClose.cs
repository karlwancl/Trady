using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class HistoricalHighestClose : HistoricalHighest<IOhlcvData, AnalyzableTick<decimal?>>
    {
        public HistoricalHighestClose(IEnumerable<IOhlcvData> inputs)
            : base(inputs, i => i.Close)
        {
        }
    }
}