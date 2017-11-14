using System;
using System.Collections.Generic;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    public class Doji<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool, TOutput>
    {
        protected Doji(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, decimal threshold = 0.1m) : base(inputs, inputMapper)
        {
            Threshold = threshold;
        }

        public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
            => Math.Abs(mappedInputs[index].Close - mappedInputs[index].Open) < Threshold * (mappedInputs[index].High - mappedInputs[index].Low);
    }

    public class DojiByTuple : Doji<(decimal Open, decimal High, decimal Low, decimal Close), bool>
    {
        public DojiByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, decimal threshold = 0.1M)
            : base(inputs, i => i, threshold)
        {
        }
    }

    public class Doji : Doji<IOhlcv, AnalyzableTick<bool>>
    {
        public Doji(IEnumerable<IOhlcv> inputs, decimal threshold = 0.1M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), threshold)
        {
        }
    }
}