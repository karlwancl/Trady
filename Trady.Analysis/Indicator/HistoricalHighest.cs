using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class HistoricalHighest<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        public HistoricalHighest(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => mappedInputs.Take(index + 1).Max();
    }

    public class HistoricalHighestByTuple : HistoricalHighest<decimal, decimal?>
    {
        public HistoricalHighestByTuple(IEnumerable<decimal> inputs)
            : base(inputs, i => i)
        {
        }
    }
}