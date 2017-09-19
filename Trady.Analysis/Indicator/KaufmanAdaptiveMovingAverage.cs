using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class KaufmanAdaptiveMovingAverage<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        private EfficiencyRatioByTuple _er;
        private GenericMovingAverage _gema;

        public KaufmanAdaptiveMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount) : base(inputs, inputMapper)
        {
            _er = new EfficiencyRatioByTuple(inputs.Select(inputMapper), periodCount);

            Func<int, decimal> sc = i =>
            {
                double erValue = Convert.ToDouble(_er[i]);
                return Convert.ToDecimal(Math.Pow(erValue * (2.0 / (emaFastPeriodCount + 1) - 2.0 / (emaSlowPeriodCount + 1)) + 2.0 / (emaSlowPeriodCount + 1), 2));
            };

            _gema = new GenericMovingAverage(
                periodCount - 1,
                i => inputs.Select(inputMapper).ElementAt(i),
                i => inputs.Select(inputMapper).ElementAt(i),
                sc,
                inputs.Count());

            PeriodCount = periodCount;
            EmaFastPeriodCount = emaFastPeriodCount;
            EmaSlowPeriodCount = emaSlowPeriodCount;
        }

        public int PeriodCount { get; }

        public int EmaFastPeriodCount { get; }

        public int EmaSlowPeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index) => _gema[index];
    }

    public class KaufmanAdaptiveMovingAverageByTuple : KaufmanAdaptiveMovingAverage<decimal, decimal?>
    {
        public KaufmanAdaptiveMovingAverageByTuple(IEnumerable<decimal> inputs, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount)
            : base(inputs, i => i, periodCount, emaFastPeriodCount, emaSlowPeriodCount)
        {
        }
    }

    public class KaufmanAdaptiveMovingAverage : KaufmanAdaptiveMovingAverage<Candle, AnalyzableTick<decimal?>>
    {
        public KaufmanAdaptiveMovingAverage(IEnumerable<Candle> inputs, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount)
            : base(inputs, i => i.Close, periodCount, emaFastPeriodCount, emaSlowPeriodCount)
        {
        }
    }
}