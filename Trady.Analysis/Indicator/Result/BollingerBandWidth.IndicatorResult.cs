using System;
using static Trady.Analysis.Indicator.BollingerBandWidth;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBandWidth : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? bandWidth) : base(dateTime, bandWidth)
            {
            }

            public decimal? BandWidth => Values[0];
        }
    }
}