using System;
using static Trady.Analysis.Indicator.StandardDeviation;

namespace Trady.Analysis.Indicator
{
    public partial class StandardDeviation : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? sd) : base(dateTime, sd)
            {
            }

            public decimal? Sd => Values[0];
        }
    }
}