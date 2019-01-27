using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IRDiffAnalyzable<TOutput>
    {
        IReadOnlyList<TOutput> ComputeRDiff(int? startIndex = null, int? endIndex = null);

        IReadOnlyList<TOutput> ComputeRDiff(IEnumerable<int> indexes);

        (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourRDiff(int index);

        TOutput RDiff(int index);
    }
}
