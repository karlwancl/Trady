using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBandWidth : AnalyzableBase<decimal, decimal?>
    {
        private BollingerBands _bb;

        public BollingerBandWidth(IList<Candle> candles, int periodCount, decimal sdCount) :
            this(candles.Select(c => (c.Close)).ToList(), periodCount, sdCount)
        {
        }

        public BollingerBandWidth(IList<decimal> closes, int periodCount, decimal sdCount) : base(closes)
        {
            _bb = new BollingerBands(closes, periodCount, sdCount);

            PeriodCount = periodCount;
            SdCount = sdCount;
        }

        public int PeriodCount { get; private set; }

        public decimal SdCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index)
        {
            var bb = _bb[index];
            return (bb.UpperBand - bb.LowerBand) / bb.MiddleBand * 100;
        }
    }
}