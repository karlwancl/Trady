using System;
using static Trady.Analysis.Indicator.EfficiencyRatio;

namespace Trady.Analysis.Indicator
{
    public partial class EfficiencyRatio : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? er) : base(dateTime, er)
            {
            }

            public decimal? Er => Values[0];
        }
    }
}