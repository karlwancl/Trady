using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class StochasticsMomentumIndex<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), TOutput>
    {
        private readonly StochasticsMomentumByTuple _sm;
        private readonly HighestHighLowestLowDifferenceByTuple _diff;
        private readonly IDictionary<int, GenericMovingAverage> _emaCache;
        private readonly Func<IAnalyzable<decimal?>, int, decimal?> _doubleEma;

        public int SmoothingPeriodA { get; }
        public int SmoothingPeriodB { get; }
        public int PeriodCount { get; }

        protected StochasticsMomentumIndex(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount, int smoothingPeriodA, int smoothingPeriodB) : base(inputs, inputMapper)
        {
            _sm = new StochasticsMomentumByTuple(inputs.Select(inputMapper), periodCount);
            _diff = new HighestHighLowestLowDifferenceByTuple(inputs.Select(inputMapper).Select(i => (i.High, i.Low)), periodCount);

            _emaCache = new Dictionary<int, GenericMovingAverage>();
            _doubleEma = (analyzable, index) =>
            {
                var innerHash = analyzable.GetHashCode();
                if (!_emaCache.TryGetValue(innerHash, out var innerEma))
                {
                    innerEma = Ema(i => analyzable[i], smoothingPeriodA);
                    _emaCache.Add(innerHash, innerEma);
                }

                var outerHash = innerEma.GetHashCode();
                if (!_emaCache.TryGetValue(outerHash, out var outerEma))
                {
                    outerEma = Ema(i => innerEma[i], smoothingPeriodB);
                    _emaCache.Add(outerHash, outerEma);
                }

                return outerEma[index];

                GenericMovingAverage Ema(Func<int, decimal?> indexFunction, int smoothingPeriod)
                    => new GenericMovingAverage(periodCount - 1, indexFunction, indexFunction, Smoothing.Ema(smoothingPeriod), inputs.Count());
            };

            PeriodCount = periodCount;
            SmoothingPeriodB = smoothingPeriodB;
            SmoothingPeriodA = smoothingPeriodA;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            var smoothedDelta = _doubleEma(_sm, index);
            var smoothedRange = _doubleEma(_diff, index) / 2;
            return smoothedRange == 0 ? default : 100 * smoothedDelta / smoothedRange;
        }
    }

    public class StochasticsMomentumIndexByTuple : StochasticsMomentumIndex<(decimal High, decimal Low, decimal Close), decimal?>
    {
        public StochasticsMomentumIndexByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smoothingPeriodA, int smoothingPeriodB) 
            : base(inputs, i => i, periodCount, smoothingPeriodA, smoothingPeriodB)
        {
        }
    }

    public class StochasticsMomentumIndex : StochasticsMomentumIndex<IOhlcv, AnalyzableTick<decimal?>>
    {
        public StochasticsMomentumIndex(IEnumerable<IOhlcv> inputs, int periodCount, int smoothingPeriodA, int smoothingPeriodB) 
            : base(inputs, i => (i.High, i.Low, i.Close), periodCount, smoothingPeriodA, smoothingPeriodB)
        {
        }
    }
}
