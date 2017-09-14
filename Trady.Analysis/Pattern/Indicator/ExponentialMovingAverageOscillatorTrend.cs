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
        private readonly ExponentialMovingAverageOscillatorByTuple _emaOsc;

        public ExponentialMovingAverageOscillatorTrend(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount1, int periodCount2) : base(inputs, inputMapper)
        {
            _emaOsc = new ExponentialMovingAverageOscillatorByTuple(inputs.Select(inputMapper), periodCount1, periodCount2);
        }

        protected override Trend? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => index >= 1 ? StateHelper.IsTrending(_emaOsc[index], _emaOsc[index - 1]) : null;
    }

    public class ExponentialMovingAverageOscillatorTrendByTuple : ExponentialMovingAverageOscillatorTrend<decimal, Trend?>
    {
        public ExponentialMovingAverageOscillatorTrendByTuple(IEnumerable<decimal> inputs, int periodCount1, int periodCount2)
            : base(inputs, i => i, periodCount1, periodCount2)
        {
        }
    }

    public class ExponentialMovingAverageOscillatorTrend : ExponentialMovingAverageOscillatorTrend<Candle, AnalyzableTick<Trend?>>
    {
        public ExponentialMovingAverageOscillatorTrend(IEnumerable<Candle> inputs, int periodCount1, int periodCount2)
            : base(inputs, i => i.Close, periodCount1, periodCount2)
        {
        }
    }
}