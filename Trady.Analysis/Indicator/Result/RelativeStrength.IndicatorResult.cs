using System;
using Trady.Core;
using static Trady.Analysis.Indicator.RelativeStrength;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrength : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? rs) : base(dateTime, rs)
            {
            }

            public decimal? Rs => Values[0];
        }
    }
}