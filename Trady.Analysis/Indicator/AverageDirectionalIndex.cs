using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class AverageDirectionalIndex<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), TOutput>
    {
        private DirectionalMovementIndexByTuple _dx;
        private readonly GenericMovingAverage _adx;

        public AverageDirectionalIndex(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _dx = new DirectionalMovementIndexByTuple(inputs.Select(inputMapper), periodCount);

            _adx = new GenericMovingAverage(
                periodCount,
                i => Enumerable.Range(i - periodCount + 1, periodCount).Select(j => _dx[j]).Average(),
                i => _dx[i],
                Smoothing.Mma(periodCount),
                inputs.Count());

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index) => _adx[index];
    }

    public class AverageDirectionalIndexByTuple : AverageDirectionalIndex<(decimal High, decimal Low, decimal Close), decimal?>
    {
        public AverageDirectionalIndexByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class AverageDirectionalIndex : AverageDirectionalIndex<IOhlcv, AnalyzableTick<decimal?>>
    {
        public AverageDirectionalIndex(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => (i.High, i.Low, i.Close), periodCount)
        {
        }
    }
}