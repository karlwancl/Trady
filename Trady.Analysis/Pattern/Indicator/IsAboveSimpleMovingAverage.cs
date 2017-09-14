using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveSimpleMovingAverage<TInput, TOutput> : AnalyzableBase<TInput, decimal, bool?, TOutput>
    {
        private readonly SimpleMovingAverageByTuple _sma;

        public IsAboveSimpleMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _sma = new SimpleMovingAverageByTuple(inputs.Select(inputMapper), periodCount);
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => mappedInputs[index].IsLargerThan(_sma[index]);
    }

    public class IsAboveSimpleMovingAverageByTuple : IsAboveSimpleMovingAverage<decimal, bool?>
    {
        public IsAboveSimpleMovingAverageByTuple(IEnumerable<decimal> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class IsAboveSimpleMovingAverage : IsAboveSimpleMovingAverage<Candle, AnalyzableTick<bool?>>
    {
        public IsAboveSimpleMovingAverage(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}