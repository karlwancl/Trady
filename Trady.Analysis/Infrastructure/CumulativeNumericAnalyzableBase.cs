using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
	public abstract class CumulativeNumericAnalyzableBase<TInput, TMappedInput, TOutput>
		: CumulativeAnalyzableBase<TInput, TMappedInput, decimal?, TOutput>, IDiffAnalyzable<TOutput>, ISmaAnalyzable<TOutput>
	{
		protected CumulativeNumericAnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper)
			: base(inputs, inputMapper)
		{
		}
		#region IDiffAnalyzable implementation

		public IReadOnlyList<TOutput> ComputeDiff(int? startIndex = default(int?), int? endIndex = default(int?)) => Compute(Diff, startIndex, endIndex);

		public IReadOnlyList<TOutput> ComputeDiff(IEnumerable<int> indexes) => Compute(Diff, indexes);

		public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourDiff(int index) => Compute(Diff, index);

		public TOutput Diff(int index) => index > 0 ? Map(i => ComputeByIndex(i) - ComputeByIndex(i - 1), index) : default(TOutput);

		#endregion

		#region ISmaAnalyzable implementation

		public IReadOnlyList<TOutput> ComputeSma(int periodCount, int? startIndex = default(int?), int? endIndex = default(int?)) => Compute(i => Sma(periodCount, i), startIndex, endIndex);

		public IReadOnlyList<TOutput> ComputeSma(int periodCount, IEnumerable<int> indexes) => Compute(i => Sma(periodCount, i), indexes);

		public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourSma(int periodCount, int index) => Compute(i => Sma(periodCount, i), index);

		public TOutput Sma(int periodCount, int index)
			=> index >= periodCount - 1 ? Map(i => Enumerable.Range(i - periodCount + 1, periodCount).Select(ComputeByIndex).Average(), index) : default(TOutput);

		#endregion
	}
}
