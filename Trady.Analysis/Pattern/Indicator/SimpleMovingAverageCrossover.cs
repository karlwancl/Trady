using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageCrossover<TInput, TOutput> : AnalyzableBase<TInput, decimal, Crossover?, TOutput>
    {
        readonly SimpleMovingAverageOscillatorByTuple _smaOsc;

        public SimpleMovingAverageCrossover(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount1, int periodCount2) : base(inputs, inputMapper)
        {
			_smaOsc = new SimpleMovingAverageOscillatorByTuple(inputs.Select(inputMapper), periodCount1, periodCount2);
		}

        protected override Crossover? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
			=> index >= 1 ? StateHelper.IsCrossover(_smaOsc[index], _smaOsc[index - 1]) : null;
	}

    public class SimpleMovingAverageCrossoverByTuple : SimpleMovingAverageCrossover<decimal, Crossover?>
    {
        public SimpleMovingAverageCrossoverByTuple(IEnumerable<decimal> inputs, int periodCount1, int periodCount2) 
            : base(inputs, i => i, periodCount1, periodCount2)
        {
        }
    }

    public class SimpleMovingAverageCrossover : SimpleMovingAverageCrossover<Candle, AnalyzableTick<Crossover?>>
    {
        public SimpleMovingAverageCrossover(IEnumerable<Candle> inputs, int periodCount1, int periodCount2) 
            : base(inputs, i => i.Close, periodCount1, periodCount2)
        {
        }
    }
}