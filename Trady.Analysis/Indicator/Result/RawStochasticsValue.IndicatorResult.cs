using System;
using System.Collections.Generic;
using Trady.Core;
using static Trady.Analysis.Indicator.RawStochasticsValue;

namespace Trady.Analysis.Indicator
{
    public partial class RawStochasticsValue : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? rsv) : base(dateTime)
            {
                Rsv = rsv;
            }

            public decimal? Rsv { get; private set; }
        }
    }
}
