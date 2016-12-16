using System;
using System.Collections.Generic;
using Trady.Core;
using static Trady.Analysis.Indicator.AccumulationDistributionLine;

namespace Trady.Analysis.Indicator
{
    public partial class AccumulationDistributionLine : CacheIndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? accumDist) : base(dateTime)
            {
                AccumDist = accumDist;
            }

            public decimal? AccumDist { get; private set; }
        }
    }
}
