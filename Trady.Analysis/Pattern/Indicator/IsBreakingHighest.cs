using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsBreakingHighest<TInput, TOutput> : AnalyzableBase<TInput, decimal, bool?, TOutput>
    {
        private readonly HighestByTuple _h;

        public IsBreakingHighest(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _h = new HighestByTuple(inputs.Select(inputMapper), periodCount);
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => index > 0 && mappedInputs[index] > _h[index - 1]; 
    }

    public class IsBreakingHighestByTuple : IsBreakingHighest<decimal, bool?>
    {
        public IsBreakingHighestByTuple(IEnumerable<decimal> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }
}
