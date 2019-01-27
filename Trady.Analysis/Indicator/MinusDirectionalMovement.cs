using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class MinusDirectionalMovement<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        public MinusDirectionalMovement(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => index > 0 ? (decimal?)mappedInputs[index - 1] - mappedInputs[index] : default;
    }

    public class MinusDirectionalMovementByTuple : MinusDirectionalMovement<decimal, decimal?>
    {
        public MinusDirectionalMovementByTuple(IEnumerable<decimal> inputs)
            : base(inputs, i => i)
        {
        }
    }

    public class MinusDirectionalMovement : MinusDirectionalMovement<IOhlcv, AnalyzableTick<decimal?>>
    {
        public MinusDirectionalMovement(IEnumerable<IOhlcv> inputs)
            : base(inputs, i => i.Low)
        {
        }
    }
}
