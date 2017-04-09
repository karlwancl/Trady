using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AverageDirectionalIndexRating : AnalyzableBase<(decimal High, decimal Low, decimal Close), decimal?>
    {
        private AverageDirectionalIndex _adx;

        public AverageDirectionalIndexRating(IList<Candle> candles, int periodCount, int adxrPeriodCount) :
            this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount, adxrPeriodCount)
        {
        }

        public AverageDirectionalIndexRating(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int adxrPeriodCount) : base(inputs)
        {
            _adx = new AverageDirectionalIndex(inputs, periodCount);

            PeriodCount = periodCount;
            AdxrPeriodCount = adxrPeriodCount;
        }

        public int PeriodCount { get; private set; }

        public int AdxrPeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index)
            => (index >= PeriodCount || index >= AdxrPeriodCount) ? (_adx[index] + _adx[index - AdxrPeriodCount]) / 2 : null;
    }
}