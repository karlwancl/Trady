using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrength : AnalyzableBase<decimal, decimal?>
    {
        private ClosePriceChange _closePriceChange;
        private GenericExponentialMovingAverage<decimal> _uEma, _dEma;

        public RelativeStrength(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public RelativeStrength(IList<decimal> closes, int periodCount) : base(closes)
        {
            _closePriceChange = new ClosePriceChange(closes);

            Func<int, decimal?> u = i => i > 0 ? Math.Max(_closePriceChange[i].Value, 0) : (decimal?)null;
            Func<int, decimal?> l = i => i > 0 ? Math.Abs(Math.Min(_closePriceChange[i].Value, 0)) : (decimal?)null;

            _uEma = new GenericExponentialMovingAverage<decimal>(
                closes,
                periodCount,
                i => i > 0 ? Enumerable.Range(i - PeriodCount + 1, PeriodCount).Average(j => u(j)) : null,
                i => u(i),
                i => 1.0m / periodCount);

            _dEma = new GenericExponentialMovingAverage<decimal>(
                closes,
                periodCount,
                i => i > 0 ? Enumerable.Range(i - PeriodCount + 1, PeriodCount).Average(j => l(j)) : null,
                i => l(i),
                i => 1.0m / periodCount);

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index) => _uEma[index] / _dEma[index];
    }
}