using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class ModifiedExponentialMovingAverage<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        private readonly GenericExponentialMovingAverage _gema;

        public ModifiedExponentialMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _gema = new GenericExponentialMovingAverage(
                0,
                i => inputs.Select(inputMapper).ElementAt(i),
                i => inputs.Select(inputMapper).ElementAt(i),
                i => 1.0m / periodCount,
                inputs.Count());

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index) => _gema[index];
    }

    public class ModifiedExponentialMovingAverageByTuple : ModifiedExponentialMovingAverage<decimal, decimal?>
    {
        public ModifiedExponentialMovingAverageByTuple(IEnumerable<decimal> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class ModifiedExponentialMovingAverage : ModifiedExponentialMovingAverage<Candle, AnalyzableTick<decimal?>>
    {
        public ModifiedExponentialMovingAverage(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}