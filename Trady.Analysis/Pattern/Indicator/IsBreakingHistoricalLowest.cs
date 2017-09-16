using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsBreakingHistoricalLowest<TInput, TOutput> : AnalyzableBase<TInput, decimal, bool?, TOutput>
    {
        private readonly HistoricalLowestByTuple _hl;

        public IsBreakingHistoricalLowest(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper) : base(inputs, inputMapper)
        {
            _hl = new HistoricalLowestByTuple(inputs.Select(inputMapper));
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => index > 0 && mappedInputs[index] < _hl[index - 1]; 
    }

    public class IsBreakingHistoricalLowestByTuple : IsBreakingHistoricalLowest<decimal, bool?>
    {
        public IsBreakingHistoricalLowestByTuple(IEnumerable<decimal> inputs)
            : base(inputs, i => i)
        {
        }
    }
}
