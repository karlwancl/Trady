using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class PlusDirectionalMovement<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public PlusDirectionalMovement(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper) : base(inputs, inputMapper, outputMapper)
        {
        }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
            => index > 0 ? mappedInputs.ElementAt(index) - mappedInputs.ElementAt(index - 1) : (decimal?)null;
    }

    public class PlusDirectionalMovementByTuple : PlusDirectionalMovement<decimal, decimal?>
    {
        public PlusDirectionalMovementByTuple(IEnumerable<decimal> inputs) 
            : base(inputs, i => i, (i, otm) => otm)
        {
        }
    }

    public class PlusDirectionalMovement : PlusDirectionalMovement<Candle, AnalyzableTick<decimal?>>
    {
        public PlusDirectionalMovement(IEnumerable<Candle> inputs) 
            : base(inputs, i => i.High, (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm))
        {
        }
    }
}