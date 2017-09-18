using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IEmaAnalyzable<TOutput> : IAnalyzable<TOutput>
    {
        IReadOnlyList<TOutput> ComputeEma(int periodCount, int? startIndex = null, int? endIndex = null);

        IReadOnlyList<TOutput> ComputeEma(int periodCount, IEnumerable<int> indexes);

        (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourEma(int periodCount, int index);

        TOutput Ema(int periodCount, int index);
    }
}