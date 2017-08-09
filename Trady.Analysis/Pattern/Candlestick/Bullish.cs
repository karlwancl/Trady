using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class Bullish<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        public Bullish(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, Func<TInput, bool, TOutput> outputMapper) : base(inputs, inputMapper, outputMapper)
        {
        }

        protected override bool ComputeByIndexImpl(IEnumerable<(decimal Open, decimal Close)> mappedInputs, int index)
            => mappedInputs.ElementAt(index).Open < mappedInputs.ElementAt(index).Close;
	}

    public class BullishByTuple : Bullish<(decimal Open, decimal Close), bool>
    {
        public BullishByTuple(IEnumerable<(decimal Open, decimal Close)> inputs) 
            : base(inputs, i => i, (i, otm) => otm)
        {
        }
    }

    public class Bullish : Bullish<Candle, AnalyzableTick<bool>>
    {
        public Bullish(IEnumerable<Candle> inputs) 
            : base(inputs, i => (i.Open, i.Close), (i, otm) => new AnalyzableTick<bool>(i.DateTime, otm))
        {
        }
    }
}
