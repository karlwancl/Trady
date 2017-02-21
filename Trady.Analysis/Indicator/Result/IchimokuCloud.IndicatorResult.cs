using System;
using Trady.Core;
using static Trady.Analysis.Indicator.IchimokuCloud;

namespace Trady.Analysis.Indicator
{
    public partial class IchimokuCloud : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? conversionLine, decimal? baseLine, decimal? leadingSpanA, decimal? leadingSpanB, decimal? laggingSpan) : base(dateTime)
            {
                ConversionLine = conversionLine;
                BaseLine = baseLine;
                LeadingSpanA = leadingSpanA;
                LeadingSpanB = leadingSpanB;
                LaggingSpan = laggingSpan;
            }

            public decimal? ConversionLine { get; private set; }

            public decimal? BaseLine { get; private set; }

            public decimal? LeadingSpanA { get; private set; }

            public decimal? LeadingSpanB { get; private set; }

            public decimal? LaggingSpan { get; private set; }
        }
    }
}