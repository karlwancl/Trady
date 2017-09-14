using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageOscillatorTrend<TInput, TOutput> : AnalyzableBase<TInput, decimal, Trend?, TOutput>
    {
        private readonly SimpleMovingAverageOscillatorByTuple _smaOsc;

        public SimpleMovingAverageOscillatorTrend(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount1, int periodCount2) : base(inputs, inputMapper)
        {
            _smaOsc = new SimpleMovingAverageOscillatorByTuple(inputs.Select(inputMapper), periodCount1, periodCount2);
        }

        protected override Trend? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => index >= 1 ? StateHelper.IsTrending(_smaOsc[index], _smaOsc[index - 1]) : null;
    }

    public class SimpleMovingAverageOscillatorTrendByTuple : SimpleMovingAverageOscillatorTrend<decimal, Trend?>
    {
        public SimpleMovingAverageOscillatorTrendByTuple(IEnumerable<decimal> inputs, int periodCount1, int periodCount2)
            : base(inputs, i => i, periodCount1, periodCount2)
        {
        }
    }

    public class SimpleMovingAverageOscillatorTrend : SimpleMovingAverageOscillatorTrend<Candle, AnalyzableTick<Trend?>>
    {
        public SimpleMovingAverageOscillatorTrend(IEnumerable<Candle> inputs, int periodCount1, int periodCount2)
            : base(inputs, i => i.Close, periodCount1, periodCount2)
        {
        }
    }
}