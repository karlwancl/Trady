using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBandWidth : IndicatorBase<decimal, decimal?>
    {
        private BollingerBands _bb;

        public BollingerBandWidth(IList<Candle> candles, int periodCount, int sdCount) :
            this(candles.Select(c => (c.Close)).ToList(), periodCount, sdCount)
        {
        }

        public BollingerBandWidth(IList<decimal> closes, int periodCount, int sdCount) : base(closes, periodCount, sdCount)
        {
            _bb = new BollingerBands(closes, periodCount, sdCount);
        }

        public int PeriodCount => Parameters[0];

        public int SdCount => Parameters[1];

        protected override decimal? ComputeByIndexImpl(int index)
        {
            var bb = _bb[index];
            return (bb.UpperBand - bb.LowerBand) / bb.MiddleBand * 100;
        }
    }
}