using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AverageTrueRange : IndicatorBase<(decimal High, decimal Low, decimal Close), decimal?>
    {
        private GenericExponentialMovingAverage<(decimal High, decimal Low, decimal Close)> _trEma;

        public AverageTrueRange(IList<Candle> candles, int periodCount) :
            this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount)
        {
        }

        public AverageTrueRange(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount) : base(inputs, periodCount)
        {
            Func<int, decimal?> tr = i => i > 0 ? new List<decimal?> {
                Math.Abs(Inputs[i].High - Inputs[i].Low),
                Math.Abs(Inputs[i].High - Inputs[i - 1].Close),
                Math.Abs(Inputs[i].Low - Inputs[i - 1].Close) }.Max() :
                null;

            _trEma = new GenericExponentialMovingAverage<(decimal High, decimal Low, decimal Close)>(
                inputs,
                periodCount,
                i => Enumerable.Range(i - periodCount + 1, periodCount).Average(j => tr(j)),
                i => tr(i),
                i => 1.0m / periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override decimal? ComputeByIndexImpl(int index) => _trEma[index];
    }
}