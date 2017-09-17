using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public abstract class NumericAnalyzableBase<TInput, TMappedInput, TOutput>
        : AnalyzableBase<TInput, TMappedInput, decimal?, TOutput>, INumericAnalyzable<TOutput>
    {
        protected NumericAnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper) : base(inputs, inputMapper)
        {
        }

        public TOutput Diff(int index) => index > 0 ? Get(i => ComputeByIndex(i) - ComputeByIndex(i - 1), index) : default(TOutput);

        public IReadOnlyList<TOutput> ComputeDiff(int? startIndex = default(int?), int? endIndex = default(int?)) => Compute(Diff, startIndex, endIndex);

        public IReadOnlyList<TOutput> ComputeDiff(IEnumerable<int> indexes) => Compute(Diff, indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourDiff(int index) => Compute(Diff, index);
    }
}
