using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class MinusDirectionalMovement<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public MinusDirectionalMovement(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
            => index > 0 ? mappedInputs.ElementAt(index - 1) - mappedInputs.ElementAt(index) : (decimal?)null;
	}

    public class MinusDirectionalMovementByTuple : MinusDirectionalMovement<decimal, decimal?>
    {
        public MinusDirectionalMovementByTuple(IEnumerable<decimal> inputs) 
            : base(inputs, i => i)
        {
        }
    }

    public class MinusDirectionalMovement : MinusDirectionalMovement<Candle, AnalyzableTick<decimal?>>
    {
        public MinusDirectionalMovement(IEnumerable<Candle> inputs) 
            : base(inputs, i => i.Low)
        {
        }
    }
}