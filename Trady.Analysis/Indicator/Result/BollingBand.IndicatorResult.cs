using System;
using static Trady.Analysis.Indicator.BollingerBands;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBands : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? lowerBand, decimal? middleBand, decimal? upperBand)
                : base(dateTime, lowerBand, middleBand, upperBand)
            {
            }

            public decimal? LowerBand => Values[0];

            public decimal? MiddleBand => Values[1];

            public decimal? UpperBand => Values[2];
        }
    }
}