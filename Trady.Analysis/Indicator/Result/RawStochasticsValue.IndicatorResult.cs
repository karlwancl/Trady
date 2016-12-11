using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RawStochasticsValue : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal rsv) : base(dateTime)
            {
                Rsv = rsv;
            }

            public decimal Rsv { get; private set; }
        }
    }
}
