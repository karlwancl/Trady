using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public abstract class NumericAnalyzableBase<TInput, TMappedInput, TOutput>
        : AnalyzableBase<TInput, TMappedInput, decimal?, TOutput>, IDiffAnalyzable<TOutput>, ISmaAnalyzable<TOutput>, IRDiffAnalyzable<TOutput>, ISdAnalyzable<TOutput>
    {
        protected NumericAnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper) : base(inputs, inputMapper)
        {
        }

        #region IDiffAnalyzable implementation

        public IReadOnlyList<TOutput> ComputeDiff(int? startIndex = default, int? endIndex = default) => Compute(Diff, startIndex, endIndex);

        public IReadOnlyList<TOutput> ComputeDiff(IEnumerable<int> indexes) => Compute(Diff, indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourDiff(int index) => Compute(Diff, index);

        public TOutput Diff(int index)
            => Map(i => index > 0 ? ComputeByIndex(i) - ComputeByIndex(i - 1) : default, index);

        #endregion

        #region ISmaAnalyzable implementation

        public IReadOnlyList<TOutput> ComputeSma(int periodCount, int? startIndex = default, int? endIndex = default) => Compute(i => Sma(periodCount, i), startIndex, endIndex);

        public IReadOnlyList<TOutput> ComputeSma(int periodCount, IEnumerable<int> indexes) => Compute(i => Sma(periodCount, i), indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourSma(int periodCount, int index) => Compute(i => Sma(periodCount, i), index);

        public TOutput Sma(int periodCount, int index)
            => Map(i => index >= periodCount - 1 ? Enumerable.Range(i - periodCount + 1, periodCount).Select(ComputeByIndex).Average() : default, index);

        #endregion  

        #region IRDiffAnalyzable implementation

        public IReadOnlyList<TOutput> ComputeRDiff(int? startIndex = default, int? endIndex = default) => Compute(RDiff, startIndex, endIndex);

        public IReadOnlyList<TOutput> ComputeRDiff(IEnumerable<int> indexes) => Compute(RDiff, indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourRDiff(int index) => Compute(RDiff, index);

        public TOutput RDiff(int index)
            => Map(i => index > 0 ? (ComputeByIndex(i) - ComputeByIndex(i - 1)) / ComputeByIndex(i - 1) * 100 : default, index);

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
            decimal? sd(int i)
            {
                if (index < periodCount - 1)
                    return default;

                IEnumerable<decimal?> items(int j) => Enumerable.Range(j - periodCount + 1, periodCount).Select(ComputeByIndex);
                var count = items(i).Count();
                var avg = items(i).Average();
                var variance = items(i).Select(item => (item - avg) * (item - avg)).Sum() / count;
                return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(variance)));
            }

            return Map(sd, index);
        }

        #endregion
    }
}
