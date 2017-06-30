using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Infrastructure
{
    public interface IAnalyzable2<TInput, TOutput>
    {
        IEnumerable<TInput> Inputs { get; }

        TOutput ComputeByIndex(int index);

        TOutput this[int index] { get; }

        IList<TOutput> Compute(int? startIndex = null, int? endIndex = null);
    }
}
