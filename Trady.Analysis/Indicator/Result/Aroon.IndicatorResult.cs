using System;
using static Trady.Analysis.Indicator.Aroon;

namespace Trady.Analysis.Indicator
{
    public partial class Aroon : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? up, decimal? down) : base(dateTime, up, down)
            {
            }

            public decimal? Up => Values[0];

            public decimal? Down => Values[1];
        }
    }
}