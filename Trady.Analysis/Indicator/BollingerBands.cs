using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBands : IndicatorBase<decimal, (decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand)>
    {
        private SimpleMovingAverage _sma;
        private StandardDeviation _sd;

        public BollingerBands(IList<Candle> candles, int periodCount, int sdCount) :
            this(candles.Select(c => (c.Close)).ToList(), periodCount, sdCount)
        {
        }

        public BollingerBands(IList<decimal> closes, int periodCount, int sdCount) : base(closes, periodCount, sdCount)
        {
            _sma = new SimpleMovingAverage(closes, periodCount);
            _sd = new StandardDeviation(closes, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public int SdCount => Parameters[1];

        protected override (decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand) ComputeByIndexImpl(int index)
        {
            decimal? middleBand = _sma[index];
            decimal? sd = _sd[index];
            return (middleBand - SdCount * sd, middleBand, middleBand + SdCount * sd);
        }
    }
}