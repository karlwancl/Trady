using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface ISdAnalyzable<TOutput>
    {
        IReadOnlyList<TOutput> ComputeSd(int periodCount, decimal sdValue, int? startIndex = null, int? endIndex = null);

        IReadOnlyList<TOutput> ComputeSd(int periodCount, decimal sdValue, IEnumerable<int> indexes);

        (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourSd(int periodCount, decimal sdValue, int index);

        TOutput Sd(int periodCount, decimal sdValue, int index);
    }
}
