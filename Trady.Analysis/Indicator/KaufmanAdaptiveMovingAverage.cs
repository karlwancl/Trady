using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class KaufmanAdaptiveMovingAverage<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        EfficiencyRatioByTuple _er;
        GenericExponentialMovingAverage _gema;

        public KaufmanAdaptiveMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount) : base(inputs, inputMapper, outputMapper)
        {
			_er = new EfficiencyRatioByTuple(inputs.Select(inputMapper), periodCount);

			Func<int, decimal> sc = i =>
			{
				double erValue = Convert.ToDouble(_er[i]);
				return Convert.ToDecimal(Math.Pow(erValue * (2.0 / (emaFastPeriodCount + 1) - 2.0 / (emaSlowPeriodCount + 1)) + 2.0 / (emaSlowPeriodCount + 1), 2));
			};

            _gema = new GenericExponentialMovingAverage(
                periodCount - 1,
                i => inputs.Select(inputMapper).ElementAt(i),
                i => inputs.Select(inputMapper).ElementAt(i),
                i => sc(i),
                inputs.Count());

			PeriodCount = periodCount;
			EmaFastPeriodCount = emaFastPeriodCount;
			EmaSlowPeriodCount = emaSlowPeriodCount;
        }

        public int PeriodCount { get; }

        public int EmaFastPeriodCount { get; }

        public int EmaSlowPeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index) => _gema[index];
    }

    public class KaufmanAdaptiveMovingAverageByTuple : KaufmanAdaptiveMovingAverage<decimal, decimal?>
    {
        public KaufmanAdaptiveMovingAverageByTuple(IEnumerable<decimal> inputs, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount) 
            : base(inputs, i => i, (i, otm) => otm, periodCount, emaFastPeriodCount, emaSlowPeriodCount)
        {
        }
    }

    public class KaufmanAdaptiveMovingAverage : KaufmanAdaptiveMovingAverage<Candle, AnalyzableTick<decimal?>>
    {
        public KaufmanAdaptiveMovingAverage(IEnumerable<Candle> inputs, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount)
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), periodCount, emaFastPeriodCount, emaSlowPeriodCount)
        {
        }
    }
}