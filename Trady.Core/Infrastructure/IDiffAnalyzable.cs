using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IDiffAnalyzable<TOutput>
    {
        IReadOnlyList<TOutput> ComputeDiff(int? startIndex = null, int? endIndex = null);

        IReadOnlyList<TOutput> ComputeDiff(IEnumerable<int> indexes);

        (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourDiff(int index);

        TOutput Diff(int index);
    }
}
