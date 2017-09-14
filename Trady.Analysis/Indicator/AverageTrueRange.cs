using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class AverageTrueRange<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), decimal?, TOutput>
    {
        private readonly TrueRangeByTuple _tr;
        private readonly GenericExponentialMovingAverage _trEma;

        public AverageTrueRange(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount)
            : base(inputs, inputMapper)
        {
            _tr = new TrueRangeByTuple(inputs.Select(inputMapper));

            _trEma = new GenericExponentialMovingAverage(
                periodCount - 1,
                i => Enumerable.Range(i - periodCount + 1, periodCount).Average(j => _tr[j]),
                i => _tr[i],
                i => 1.0m / periodCount,
                inputs.Count());

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index) => _trEma[index];
    }

    public class AverageTrueRangeByTuple : AverageTrueRange<(decimal High, decimal Low, decimal Close), decimal?>
    {
        public AverageTrueRangeByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class AverageTrueRange : AverageTrueRange<Candle, AnalyzableTick<decimal?>>
    {
        public AverageTrueRange(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => (i.High, i.Low, i.Close), periodCount)
        {
        }
    }
}