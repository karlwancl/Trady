using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsBreakingHistoricalHighest<TInput, TOutput> : AnalyzableBase<TInput, decimal, bool?, TOutput>
    {
        private readonly HistoricalHighestByTuple _hh;

        public IsBreakingHistoricalHighest(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper) : base(inputs, inputMapper)
        {
            _hh = new HistoricalHighestByTuple(inputs.Select(inputMapper));
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => index > 0 && mappedInputs[index] > _hh[index - 1];
    }

    public class IsBreakingHistoricalHighestByTuple : IsBreakingHistoricalHighest<decimal, bool?>
    {
        public IsBreakingHistoricalHighestByTuple(IEnumerable<decimal> inputs)
            : base(inputs, i => i)
        {
        }
    }
}
