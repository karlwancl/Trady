using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IAnalyzable
    {
        IList<object> Inputs { get; }

        object ComputeByIndex(int index);
    }

    public interface IAnalyzable<TInput, TOutput> : IAnalyzable
    {
        new IList<TInput> Inputs { get; }

        new TOutput ComputeByIndex(int index);

        TOutput this[int index] { get; }

        IList<TOutput> Compute(int? startIndex = null, int? endIndex = null);
    }
}