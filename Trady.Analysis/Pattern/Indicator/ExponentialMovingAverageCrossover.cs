using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageCrossover<TInput, TOutput> : AnalyzableBase<TInput, decimal, Crossover?, TOutput>
    {
        readonly ExponentialMovingAverageOscillatorByTuple _emaOsc;

        public ExponentialMovingAverageCrossover(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, Crossover?, TOutput> outputMapper, int periodCount1, int periodCount2) : base(inputs, inputMapper, outputMapper)
        {
			_emaOsc = new ExponentialMovingAverageOscillatorByTuple(inputs.Select(inputMapper), periodCount1, periodCount2);
		}

        protected override Crossover? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
			=> index >= 1 ? StateHelper.IsCrossover(_emaOsc[index], _emaOsc[index - 1]) : null;
	}

    public class ExponentialMovingAverageCrossoverByTuple : ExponentialMovingAverageCrossover<decimal, Crossover?>
    {
        public ExponentialMovingAverageCrossoverByTuple(IEnumerable<decimal> inputs, int periodCount1, int periodCount2) 
            : base(inputs, i => i, (i, otm) => otm, periodCount1, periodCount2)
        {
        }
    }

    public class ExponentialMovingAverageCrossover : ExponentialMovingAverageCrossover<Candle, AnalyzableTick<Crossover?>>
    {
        public ExponentialMovingAverageCrossover(IEnumerable<Candle> inputs, int periodCount1, int periodCount2) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<Crossover?>(i.DateTime, otm), periodCount1, periodCount2)
        {
        }
    }
}