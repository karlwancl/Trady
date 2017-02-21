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
                Lower = lowerBand;
                Middle = middleBand;
                Upper = upperBand;
                Width = bandWidth;
            }

            public decimal? Lower { get; private set; }

            public decimal? Middle { get; private set; }

            public decimal? Upper { get; private set; }

            public decimal? Width { get; private set; }
        }
    }
}