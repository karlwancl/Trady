using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsBreakingLowest<TInput, TOutput> : AnalyzableBase<TInput, decimal, bool?, TOutput>
    {
        private readonly LowestByTuple _l;

        public IsBreakingLowest(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _l = new LowestByTuple(inputs.Select(inputMapper), periodCount);
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => index > 0 && mappedInputs[index] < _l[index - 1]; 
    }

    public class IsBreakingLowestByTuple : IsBreakingLowest<decimal, bool?>
    {
        public IsBreakingLowestByTuple(IEnumerable<decimal> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }
}
