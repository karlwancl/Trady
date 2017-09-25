using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IPcDiffAnalyzable<TOutput>
    {
        IReadOnlyList<TOutput> ComputePcDiff(int? startIndex = null, int? endIndex = null);

        IReadOnlyList<TOutput> ComputePcDiff(IEnumerable<int> indexes);

        (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourPcDiff(int index);

        TOutput PcDiff(int index);
    }
}
