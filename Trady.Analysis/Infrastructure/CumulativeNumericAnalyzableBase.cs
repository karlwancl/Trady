using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
	public abstract class CumulativeNumericAnalyzableBase<TInput, TMappedInput, TOutput>
		: CumulativeAnalyzableBase<TInput, TMappedInput, decimal?, TOutput>, IDiffAnalyzable<TOutput>, ISmaAnalyzable<TOutput>, IRDiffAnalyzable<TOutput>, ISdAnalyzable<TOutput>
	{
		protected CumulativeNumericAnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper)
			: base(inputs, inputMapper)
		{
		}

		#region IDiffAnalyzable implementation

		public IReadOnlyList<TOutput> ComputeDiff(int? startIndex = default, int? endIndex = default) => Compute(Diff, startIndex, endIndex);

		public IReadOnlyList<TOutput> ComputeDiff(IEnumerable<int> indexes) => Compute(Diff, indexes);

		public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourDiff(int index) => Compute(Diff, index);

		public TOutput Diff(int index) => index > 0 ? Map(i => ComputeByIndex(i) - ComputeByIndex(i - 1), index) : default;

		#endregion

		#region ISmaAnalyzable implementation

		public IReadOnlyList<TOutput> ComputeSma(int periodCount, int? startIndex = default, int? endIndex = default) => Compute(i => Sma(periodCount, i), startIndex, endIndex);

		public IReadOnlyList<TOutput> ComputeSma(int periodCount, IEnumerable<int> indexes) => Compute(i => Sma(periodCount, i), indexes);

		public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourSma(int periodCount, int index) => Compute(i => Sma(periodCount, i), index);

		public TOutput Sma(int periodCount, int index)
			=> index >= periodCount - 1 ? Map(i => Enumerable.Range(i - periodCount + 1, periodCount).Select(ComputeByIndex).Average(), index) : default;

		#endregion

        #region IRDiffAnalyzable implementation

        public IReadOnlyList<TOutput> ComputeRDiff(int? startIndex = default, int? endIndex = default) => Compute(RDiff, startIndex, endIndex);

        public IReadOnlyList<TOutput> ComputeRDiff(IEnumerable<int> indexes) => Compute(RDiff, indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourRDiff(int index) => Compute(RDiff, index);

        public TOutput RDiff(int index)
            => index > 0 ? Map(i => (ComputeByIndex(i) - ComputeByIndex(i - 1)) / ComputeByIndex(i - 1) * 100, index) : default;

		#endregion

		#region ISdAnalyzable implementation

		public IReadOnlyList<TOutput> ComputeSd(int periodCount, int? startIndex = default, int? endIndex = default)
			=> Compute(i => Sd(periodCount, i), startIndex, endIndex);

		public IReadOnlyList<TOutput> ComputeSd(int periodCount, IEnumerable<int> indexes)
			=> Compute(i => Sd(periodCount, i), indexes);

		public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourSd(int periodCount, int index)
			=> Compute(i => Sd(periodCount, i), index);

		public TOutput Sd(int periodCount, int index)
		{
			if (index < periodCount - 1)
                return default;

            decimal? sd(int i)
            {
                IEnumerable<decimal?> items(int j) => Enumerable.Range(j - periodCount + 1, periodCount).Select(ComputeByIndex);
                var count = items(i).Count();
                var avg = items(i).Average();
                var diffSum = items(i).Select(item => (item - avg) * (item - avg)).Sum();
                return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(diffSum / count)));
            }

            return Map(sd, index);
		}

		#endregion
	}
}
