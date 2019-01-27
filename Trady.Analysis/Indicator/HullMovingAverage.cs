using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class HullMovingAverage<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        private readonly WeightedMovingAverageByTuple _halfPeriodWma, _fullPeriodWma;

        public HullMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount)
            : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
            _halfPeriodWma = new WeightedMovingAverageByTuple(inputs.Select(inputMapper).ToList(), Convert.ToInt32(Math.Round((periodCount * 1.0) / 2)));
            _fullPeriodWma = new WeightedMovingAverageByTuple(inputs.Select(inputMapper).ToList(), periodCount);
            SqrtFactor = Convert.ToInt32(Math.Round(Math.Sqrt(Convert.ToDouble(periodCount))));
            TriangularSqrtFactor = (1 + SqrtFactor) * SqrtFactor / 2;
        }

        private int SqrtFactor { get; }

        private int TriangularSqrtFactor { get; }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
        {
            if (index < PeriodCount + SqrtFactor - 2)
                return default;

            decimal? preWmaValue(int i) => 2 * _halfPeriodWma[i] - _fullPeriodWma[i];
            decimal? weightedPreWmaValue(int i) => (i - index + SqrtFactor) * preWmaValue(i) / TriangularSqrtFactor;
            return Enumerable.Range(index - SqrtFactor + 1, SqrtFactor).Select(weightedPreWmaValue).Sum();
        }
    }

    public class HullMovingAverageByTuple : HullMovingAverage<decimal?, decimal?>
    {
        public HullMovingAverageByTuple(IEnumerable<decimal?> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class HullMovingAverage : HullMovingAverage<IOhlcv, AnalyzableTick<decimal?>>
    {
        public HullMovingAverage(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}
