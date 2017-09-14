using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class Bullish<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        public Bullish(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override bool ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal Close)> mappedInputs, int index)
            => mappedInputs[index].Open < mappedInputs[index].Close;
    }

    public class BullishByTuple : Bullish<(decimal Open, decimal Close), bool>
    {
        public BullishByTuple(IEnumerable<(decimal Open, decimal Close)> inputs)
            : base(inputs, i => i)
        {
        }
    }

    public class Bullish : Bullish<Candle, AnalyzableTick<bool>>
    {
        public Bullish(IEnumerable<Candle> inputs)
            : base(inputs, i => (i.Open, i.Close))
        {
        }
    }
}