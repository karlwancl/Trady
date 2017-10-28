using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class HighestHigh : Highest<IOhlcvData, AnalyzableTick<decimal?>>
    {
        public HighestHigh(IEnumerable<IOhlcvData> inputs, int periodCount)
            : base(inputs, i => i.High, periodCount)
        {
        }
    }
}