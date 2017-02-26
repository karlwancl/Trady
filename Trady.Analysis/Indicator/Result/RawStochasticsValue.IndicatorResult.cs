using System;
using static Trady.Analysis.Indicator.RawStochasticsValue;

namespace Trady.Analysis.Indicator
{
    public partial class RawStochasticsValue : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? rsv) : base(dateTime, rsv)
            {
            }

            public decimal? Rsv => Values[0];
        }
    }
}