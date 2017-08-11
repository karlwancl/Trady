using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class ModifiedExponentialMovingAverage<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        readonly GenericExponentialMovingAverage _gema;

        public ModifiedExponentialMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount) : base(inputs, inputMapper, outputMapper)
        {
            _gema = new GenericExponentialMovingAverage(
			    0,
			    i => inputs.Select(inputMapper).ElementAt(i),
			    i => inputs.Select(inputMapper).ElementAt(i),
			    i => 1.0m / periodCount,
                inputs.Count());

			PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index) => _gema[index];
    }

    public class ModifiedExponentialMovingAverageByTuple : ModifiedExponentialMovingAverage<decimal, decimal?>
    {
        public ModifiedExponentialMovingAverageByTuple(IEnumerable<decimal> inputs, int periodCount) 
            : base(inputs, i => i, (i, otm) => otm, periodCount)
        {
        }
    }

    public class ModifiedExponentialMovingAverage : ModifiedExponentialMovingAverage<Candle, AnalyzableTick<decimal?>>
    {
        public ModifiedExponentialMovingAverage(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), periodCount)
        {
        }
    }
}