using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageOscillatorTrend<TInput, TOutput> : AnalyzableBase<TInput, decimal, Trend?, TOutput>
    {
        readonly ExponentialMovingAverageOscillatorByTuple _emaOsc;

        public ExponentialMovingAverageOscillatorTrend(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, Trend?, TOutput> outputMapper, int periodCount1, int periodCount2) : base(inputs, inputMapper, outputMapper)
        {
			_emaOsc = new ExponentialMovingAverageOscillatorByTuple(inputs.Select(inputMapper), periodCount1, periodCount2);
		}

        protected override Trend? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
			=> index >= 1 ? StateHelper.IsTrending(_emaOsc[index], _emaOsc[index - 1]) : null;
	}

    public class ExponentialMovingAverageOscillatorTrendByTuple : ExponentialMovingAverageOscillatorTrend<decimal, Trend?>
    {
        public ExponentialMovingAverageOscillatorTrendByTuple(IEnumerable<decimal> inputs, int periodCount1, int periodCount2) 
            : base(inputs, i => i, (i, otm) => otm, periodCount1, periodCount2)
        {
        }
    }

    public class ExponentialMovingAverageOscillatorTrend : ExponentialMovingAverageOscillatorTrend<Candle, AnalyzableTick<Trend?>>
    {
        public ExponentialMovingAverageOscillatorTrend(IEnumerable<Candle> inputs, int periodCount1, int periodCount2) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<Trend?>(i.DateTime, otm), periodCount1, periodCount2)
        {
        }
    }
}