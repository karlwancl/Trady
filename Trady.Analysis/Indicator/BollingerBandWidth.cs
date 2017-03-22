using Trady.Core;
using static Trady.Analysis.Indicator.BollingerBandWidth;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBandWidth : IndicatorBase<IndicatorResult>
    {
        private BollingerBands _bbIndicator;

        public BollingerBandWidth(Equity equity, int periodCount, int sdCount) : base(equity, periodCount, sdCount)
        {
            _bbIndicator = new BollingerBands(equity, periodCount, sdCount);
        }

        public int PeriodCount => Parameters[0];

        public int SdCount => Parameters[1];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            var bb = _bbIndicator.ComputeByIndex(index);
            var bandWidth = (bb.UpperBand - bb.LowerBand) / bb.MiddleBand * 100;
            return new IndicatorResult(Equity[index].DateTime, bandWidth);
        }
    }
}