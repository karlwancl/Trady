using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AverageDirectionalIndexRating : IndicatorBase<(decimal High, decimal Low, decimal Close), decimal?>
    {
        private AverageDirectionalIndex _adx;

        public AverageDirectionalIndexRating(IList<Candle> candles, int periodCount, int adxrPeriodCount) :
            this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount, adxrPeriodCount)
        {
        }

        public AverageDirectionalIndexRating(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int adxrPeriodCount) : base(inputs, periodCount, adxrPeriodCount)
        {
            _adx = new AverageDirectionalIndex(inputs, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public int AdxrPeriodCount => Parameters[1];

        protected override decimal? ComputeByIndexImpl(int index)
            => (index >= PeriodCount || index >= AdxrPeriodCount) ? (_adx[index] + _adx[index - AdxrPeriodCount]) / 2 : null;
    }
}