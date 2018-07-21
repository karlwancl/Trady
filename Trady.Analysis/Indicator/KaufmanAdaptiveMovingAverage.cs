using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class KaufmanAdaptiveMovingAverage<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        private EfficiencyRatioByTuple _er;
        private readonly GenericMovingAverage _gma;

        public KaufmanAdaptiveMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount) : base(inputs, inputMapper)
        {
            _er = new EfficiencyRatioByTuple(inputs.Select(inputMapper), periodCount);

            decimal smoothingFactor(int i)
            {
                var s = Smoothing.Ema(emaSlowPeriodCount)(i) + _er[i].Value * (Smoothing.Ema(emaFastPeriodCount)(i) - Smoothing.Ema(emaSlowPeriodCount)(i));
                return s * s;
            }

            _gma = new GenericMovingAverage(
                periodCount - 1,
                i => inputs.Select(inputMapper).ElementAt(i),
                i => inputs.Select(inputMapper).ElementAt(i),
                smoothingFactor,
                inputs.Count());

            PeriodCount = periodCount;
            EmaFastPeriodCount = emaFastPeriodCount;
            EmaSlowPeriodCount = emaSlowPeriodCount;
        }

        public int PeriodCount { get; }

        public int EmaFastPeriodCount { get; }

        public int EmaSlowPeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index) => _gma[index];
    }

    public class KaufmanAdaptiveMovingAverageByTuple : KaufmanAdaptiveMovingAverage<decimal, decimal?>
    {
        public KaufmanAdaptiveMovingAverageByTuple(IEnumerable<decimal> inputs, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount)
            : base(inputs, i => i, periodCount, emaFastPeriodCount, emaSlowPeriodCount)
        {
        }
    }

    public class KaufmanAdaptiveMovingAverage : KaufmanAdaptiveMovingAverage<IOhlcv, AnalyzableTick<decimal?>>
    {
        public KaufmanAdaptiveMovingAverage(IEnumerable<IOhlcv> inputs, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount)
            : base(inputs, i => i.Close, periodCount, emaFastPeriodCount, emaSlowPeriodCount)
        {
        }
    }
}
