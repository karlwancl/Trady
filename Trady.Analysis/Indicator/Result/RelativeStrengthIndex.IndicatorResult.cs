using System;
using static Trady.Analysis.Indicator.RelativeStrengthIndex;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrengthIndex : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? rsi) : base(dateTime, rsi)
            {
            }

            public decimal? Rsi => Values[0];
        }
    }
}