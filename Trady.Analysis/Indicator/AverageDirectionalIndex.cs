using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class AverageDirectionalIndex : AnalyzableBase<(decimal High, decimal Low, decimal Close), decimal?>
    {
        private DirectionalMovementIndex _dx;
        private GenericExponentialMovingAverage<(decimal High, decimal Low, decimal Close)> _adx;

        public AverageDirectionalIndex(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount)
        {
        }

        public AverageDirectionalIndex(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount) : base(inputs)
        {
            _dx = new DirectionalMovementIndex(inputs, periodCount);

            _adx = new GenericExponentialMovingAverage<(decimal High, decimal Low, decimal Close)>(
                inputs,
                periodCount,
                i => Enumerable.Range(i - periodCount + 1, periodCount).Select(j => _dx[j]).Average(),
                i => _dx[i],
                i => 1.0m / periodCount);

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index) => _adx[index];
    }
}