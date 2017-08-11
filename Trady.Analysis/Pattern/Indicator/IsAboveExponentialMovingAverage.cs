using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveExponentialMovingAverage<TInput, TOutput> : AnalyzableBase<TInput, decimal, bool?, TOutput>
    {
        readonly ExponentialMovingAverageByTuple _ema;

        public IsAboveExponentialMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, bool?, TOutput> outputMapper, int periodCount) : base(inputs, inputMapper, outputMapper)
        {
			_ema = new ExponentialMovingAverageByTuple(inputs.Select(inputMapper), periodCount);
		}

        protected override bool? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
            => mappedInputs.ElementAt(index).IsLargerThan(_ema[index]);
	}

    public class IsAboveExponentialMovingAverageByTuple : IsAboveExponentialMovingAverage<decimal, bool?>
    {
        public IsAboveExponentialMovingAverageByTuple(IEnumerable<decimal> inputs, int periodCount) 
            : base(inputs, i => i, (i, otm) => otm, periodCount)
        {
        }
    }

    public class IsAboveExponentialMovingAverage : IsAboveExponentialMovingAverage<Candle, AnalyzableTick<bool?>>
    {
        public IsAboveExponentialMovingAverage(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<bool?>(i.DateTime, otm), periodCount)
        {
        }
    }
}