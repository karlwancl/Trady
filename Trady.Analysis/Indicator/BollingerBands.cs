using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBands : AnalyzableBase<decimal, (decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand)>
    {
        private SimpleMovingAverage _sma;
        private StandardDeviation _sd;

        public BollingerBands(IList<Candle> candles, int periodCount, decimal sdCount) :
            this(candles.Select(c => (c.Close)).ToList(), periodCount, sdCount)
        {
        }

        public BollingerBands(IList<decimal> closes, int periodCount, decimal sdCount) : base(closes)
        {
            _sma = new SimpleMovingAverage(closes, periodCount);
            _sd = new StandardDeviation(closes, periodCount);

            PeriodCount = periodCount;
            SdCount = sdCount;
        }

        public int PeriodCount { get; private set; }

        public decimal SdCount { get; private set; }

        protected override (decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand) ComputeByIndexImpl(int index)
        {
            decimal? middleBand = _sma[index];
            decimal? sd = _sd[index];
            return (middleBand - SdCount * sd, middleBand, middleBand + SdCount * sd);
        }
    }
}