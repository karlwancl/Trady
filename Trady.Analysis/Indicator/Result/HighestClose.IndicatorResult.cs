using System;
using static Trady.Analysis.Indicator.HighestClose;

namespace Trady.Analysis.Indicator
{
    public partial class HighestClose : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? highestClose) : base(dateTime, highestClose)
            {
            }

            public decimal? HighestClose => Values[0];
        }
    }
}