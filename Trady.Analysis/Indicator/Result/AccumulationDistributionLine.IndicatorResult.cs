using System;
using Trady.Core;
using static Trady.Analysis.Indicator.AccumulationDistributionLine;

namespace Trady.Analysis.Indicator
{
    public partial class AccumulationDistributionLine : CummulativeIndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? accumDist) : base(dateTime, accumDist)
            {
            }

            public decimal? AccumDist => Values[0];
        }
    }
}