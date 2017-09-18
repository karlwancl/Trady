using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IMemaAnalyzable<TOutput> : IAnalyzable<TOutput>
    {
        IReadOnlyList<TOutput> ComputeMema(int periodCount, int? startIndex = null, int? endIndex = null);

        IReadOnlyList<TOutput> ComputeMema(int periodCount, IEnumerable<int> indexes);

        (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourMema(int periodCount, int index);

        TOutput Mema(int periodCount, int index);
    }
}