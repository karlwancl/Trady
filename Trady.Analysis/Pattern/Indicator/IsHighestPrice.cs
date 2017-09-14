using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsHighestPrice<TInput, TOutput> : AnalyzableBase<TInput, decimal, bool?, TOutput>
    {
        private readonly int _periodCount;

        public IsHighestPrice(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _periodCount = periodCount;
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => mappedInputs.Skip(mappedInputs.Count - _periodCount).Max() == mappedInputs[index];
    }

    public class IsHighestPriceByTuple : IsHighestPrice<decimal, bool?>
    {
        public IsHighestPriceByTuple(IEnumerable<decimal> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class IsHighestPrice : IsHighestPrice<Candle, AnalyzableTick<bool?>>
    {
        public IsHighestPrice(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}