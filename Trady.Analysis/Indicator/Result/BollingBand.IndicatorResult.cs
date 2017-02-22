using System;
using Trady.Core;
using static Trady.Analysis.Indicator.BollingerBands;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBands : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? lowerBand, decimal? middleBand, decimal? upperBand, decimal? bandWidth) : base(dateTime)
            {
                LowerBand = lowerBand;
                MiddleBand = middleBand;
                UpperBand = upperBand;
                BandWidth = bandWidth;
            }

            public decimal? LowerBand { get; private set; }

            public decimal? MiddleBand { get; private set; }

            public decimal? UpperBand { get; private set; }

            public decimal? BandWidth { get; private set; }
        }
    }
}