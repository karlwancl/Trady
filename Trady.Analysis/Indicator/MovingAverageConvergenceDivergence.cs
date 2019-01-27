using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class MovingAverageConvergenceDivergence<TInput, TOutput> 
        : AnalyzableBase<TInput, decimal, (decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram), TOutput>
    {
        private ExponentialMovingAverageOscillatorByTuple _macd;
        private readonly GenericMovingAverage _signal;

        public MovingAverageConvergenceDivergence(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) : base(inputs, inputMapper)
        {
            _macd = new ExponentialMovingAverageOscillatorByTuple(inputs.Select(inputMapper), emaPeriodCount1, emaPeriodCount2);

            _signal = new GenericMovingAverage(
                i => _macd[i],
                Smoothing.Ema(demPeriodCount),
                inputs.Count());

            EmaPeriodCount1 = emaPeriodCount1;
            EmaPeriodCount2 = emaPeriodCount2;
            DemPeriodCount = demPeriodCount;
        }

        public int EmaPeriodCount1 { get; }

        public int EmaPeriodCount2 { get; }

        public int DemPeriodCount { get; }

        protected override (decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram) ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
        {
            var macd = _macd[index];
            var signal = _signal[index];
            return (macd, signal, macd - signal);
        }
    }

    public class MovingAverageConvergenceDivergenceByTuple : MovingAverageConvergenceDivergence<decimal, (decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram)>
    {
        public MovingAverageConvergenceDivergenceByTuple(IEnumerable<decimal> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(inputs, i => i, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }
    }

    public class MovingAverageConvergenceDivergence : MovingAverageConvergenceDivergence<IOhlcv, AnalyzableTick<(decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram)>>
    {
        public MovingAverageConvergenceDivergence(IEnumerable<IOhlcv> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(inputs, i => i.Close, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }
    }
}
