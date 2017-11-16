using System;
using System.Collections.Generic;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    public class Bearish<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        public Bearish(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override bool ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal Close)> mappedInputs, int index)
            => mappedInputs[index].Open > mappedInputs[index].Close;
    }

    public class BearishByTuple : Bearish<(decimal Open, decimal Close), bool>
    {
        public BearishByTuple(IEnumerable<(decimal Open, decimal Close)> inputs)
            : base(inputs, i => i)
        {
        }
    }

    public class Bearish : Bearish<IOhlcv, AnalyzableTick<bool>>
    {
        public Bearish(IEnumerable<IOhlcv> inputs)
            : base(inputs, i => (i.Open, i.Close))
        {
        }
    }
}