using System;
using System.Collections.Generic;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public abstract class CumulativeNumericAnalyzableBase<TInput, TMappedInput, TOutput>
        : CumulativeAnalyzableBase<TInput, TMappedInput, decimal?, TOutput>, INumericAnalyzable<TOutput>
    {
        protected CumulativeNumericAnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper) 
            : base(inputs, inputMapper)
        {
        }

		public TOutput Diff(int index) => index > 0 ? Get(i => ComputeByIndex(i) - ComputeByIndex(i - 1), index) : default(TOutput);

		public IReadOnlyList<TOutput> ComputeDiff(int? startIndex = default(int?), int? endIndex = default(int?)) => Compute(Diff, startIndex, endIndex);

        public IReadOnlyList<TOutput> ComputeDiff(IEnumerable<int> indexes) => Compute(Diff, indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourDiff(int index) => ComputeNeighbour(index);
    }
}
