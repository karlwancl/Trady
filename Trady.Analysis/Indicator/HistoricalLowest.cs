using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class HistoricalLowest<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        public HistoricalLowest(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => mappedInputs.Take(index + 1).Min();
    }

    public class HistoricalLowestByTuple : HistoricalLowest<decimal, decimal?>
    {
        public HistoricalLowestByTuple(IEnumerable<decimal> inputs)
            : base(inputs, i => i)
        {
        }
    }
}