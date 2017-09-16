using System;
using System.Collections.Generic;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public abstract class NumericAnalyzableBase<TInput, TMappedInput, TOutput>
        : AnalyzableBase<TInput, TMappedInput, decimal?, TOutput>, INumericAnalyzable<TOutput>
    {
        protected NumericAnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper) : base(inputs, inputMapper)
        {
        }

        public TOutput Change(int index) => index > 0 ? Get(i => ComputeByIndex(i) - ComputeByIndex(i - 1), index) : default(TOutput);

        public IReadOnlyList<TOutput> ComputeChange(int? startIndex = default(int?), int? endIndex = default(int?)) => Compute(Change, startIndex, endIndex);
    }
}
