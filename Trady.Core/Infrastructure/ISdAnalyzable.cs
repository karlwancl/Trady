using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface ISdAnalyzable<TOutput>
    {
        IReadOnlyList<TOutput> ComputeSd(int periodCount, int? startIndex = null, int? endIndex = null);

        IReadOnlyList<TOutput> ComputeSd(int periodCount, IEnumerable<int> indexes);

        (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourSd(int periodCount, int index);

        TOutput Sd(int periodCount, int index);
    }
}
