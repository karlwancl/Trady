using System;
using static Trady.Analysis.Indicator.ChandelierExit;

namespace Trady.Analysis.Indicator
{
    public partial class ChandelierExit : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? @long, decimal? @short) : base(dateTime, @long, @short)
            {
            }

            public decimal? Long => Values[0];

            public decimal? Short => Values[1];
        }
    }
}