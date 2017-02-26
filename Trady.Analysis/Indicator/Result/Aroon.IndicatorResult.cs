using System;
using static Trady.Analysis.Indicator.Aroon;

namespace Trady.Analysis.Indicator
{
    public partial class Aroon : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? up, decimal? down, decimal? osc) : base(dateTime, up, down, osc)
            {
            }

            public decimal? Up => Values[0];

            public decimal? Down => Values[1];

            public decimal? Osc => Values[2];
        }
    }
}