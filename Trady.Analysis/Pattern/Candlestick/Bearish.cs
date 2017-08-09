using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class Bearish<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        public Bearish(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, Func<TInput, bool, TOutput> outputMapper) : base(inputs, inputMapper, outputMapper)
        {
        }

        protected override bool ComputeByIndexImpl(IEnumerable<(decimal Open, decimal Close)> mappedInputs, int index)
            => mappedInputs.ElementAt(index).Open > mappedInputs.ElementAt(index).Close;
	}

    public class BearishByTuple : Bearish<(decimal Open, decimal Close), bool>
    {
        public BearishByTuple(IEnumerable<(decimal Open, decimal Close)> inputs) 
            : base(inputs, i => i, (i, otm) => otm)
        {
        }
    }

    public class Bearish : Bearish<Candle, AnalyzableTick<bool>>
    {
        public Bearish(IEnumerable<Candle> inputs) 
            : base(inputs, i => (i.Open, i.Close), (i, otm) => new AnalyzableTick<bool>(i.DateTime, otm))
        {
        }
    }
}
