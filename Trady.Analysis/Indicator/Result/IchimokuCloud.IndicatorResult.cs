using System;
using static Trady.Analysis.Indicator.IchimokuCloud;

namespace Trady.Analysis.Indicator
{
    public partial class IchimokuCloud : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? conversionLine, decimal? baseLine, decimal? leadingSpanA, decimal? leadingSpanB, decimal? laggingSpan)
                : base(dateTime, conversionLine, baseLine, leadingSpanA, leadingSpanB, laggingSpan)
            {
            }

            public decimal? ConversionLine => Values[0];

            public decimal? BaseLine => Values[1];

            public decimal? LeadingSpanA => Values[2];

            public decimal? LeadingSpanB => Values[3];

            public decimal? LaggingSpan => Values[4];
        }
    }
}