using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AccumulationDistributionLine : CachedIndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal accumDist) : base(dateTime)
            {
                AccumDist = accumDist;
            }

            public decimal AccumDist { get; private set; }
        }
    }
}
