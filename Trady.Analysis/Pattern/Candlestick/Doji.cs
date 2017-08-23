using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class Doji<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool, TOutput>
    {
        protected Doji(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, decimal threshold = 0.1m) : base(inputs, inputMapper)
        {
            Threshold = threshold;
        }

        public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
            => Math.Abs(mappedInputs.ElementAt(index).Close - mappedInputs.ElementAt(index).Open) < Threshold * (mappedInputs.ElementAt(index).High - mappedInputs.ElementAt(index).Low);
	}

    public class DojiByTuple : Doji<(decimal Open, decimal High, decimal Low, decimal Close), bool>
    {
        public DojiByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, decimal threshold = 0.1M) 
            : base(inputs, i => i, threshold)
        {
        }
    }

    public class Doji : Doji<Candle, AnalyzableTick<bool>>
    {
        public Doji(IEnumerable<Candle> inputs, decimal threshold = 0.1M) 
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), threshold)
        {
        }
    }
}