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

        public IsAboveExponentialMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
			_ema = new ExponentialMovingAverageByTuple(inputs.Select(inputMapper), periodCount);
		}

        protected override bool? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
            => mappedInputs.ElementAt(index).IsLargerThan(_ema[index]);
	}

    public class IsAboveExponentialMovingAverageByTuple : IsAboveExponentialMovingAverage<decimal, bool?>
    {
        public IsAboveExponentialMovingAverageByTuple(IEnumerable<decimal> inputs, int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class IsAboveExponentialMovingAverage : IsAboveExponentialMovingAverage<Candle, AnalyzableTick<bool?>>
    {
        public IsAboveExponentialMovingAverage(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}