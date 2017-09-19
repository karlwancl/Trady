using System;
using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface ISmaAnalyzable<TOutput>
	{
		IReadOnlyList<TOutput> ComputeSma(int periodCount, int? startIndex = null, int? endIndex = null);

		IReadOnlyList<TOutput> ComputeSma(int periodCount, IEnumerable<int> indexes);

		(TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourSma(int periodCount, int index);

		TOutput Sma(int periodCount, int index);
	}
}
