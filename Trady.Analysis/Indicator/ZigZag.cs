using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class ZigZag<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        readonly decimal threshold;

        protected ZigZag(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, decimal threshold = 0.03m) : base(inputs, inputMapper)
        {
            this.threshold = threshold;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
        {
            throw new NotImplementedException();
        }
    }
}
