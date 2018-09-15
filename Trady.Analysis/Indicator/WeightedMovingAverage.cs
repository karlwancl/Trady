using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class WeightedMovingAverage<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        public WeightedMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount)
            : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
            TriangularFactor = (1 + PeriodCount) * PeriodCount / 2;
        }

        private int TriangularFactor { get; }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
        {
            if (index < PeriodCount - 1)
                return default;

            decimal? weightedValue(int i) => (i - index + PeriodCount) * mappedInputs[i] / TriangularFactor;
            return Enumerable.Range(index - PeriodCount + 1, PeriodCount).Select(weightedValue).Sum();
        }
    }

    public class WeightedMovingAverageByTuple : WeightedMovingAverage<decimal?, decimal?>
    {
        public WeightedMovingAverageByTuple(IEnumerable<decimal?> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class WeightedMovingAverage : WeightedMovingAverage<IOhlcv, AnalyzableTick<decimal?>>
    {
        public WeightedMovingAverage(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }

}
