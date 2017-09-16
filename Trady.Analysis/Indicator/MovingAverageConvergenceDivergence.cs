using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class MovingAverageConvergenceDivergence<TInput, TOutput> 
        : AnalyzableBase<TInput, decimal, (decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram), TOutput>
    {
        private ExponentialMovingAverageByTuple _ema1, _ema2;
        private readonly GenericExponentialMovingAverage _signal;
        private readonly Func<int, decimal?> _macd;

        public MovingAverageConvergenceDivergence(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) : base(inputs, inputMapper)
        {
            _ema1 = new ExponentialMovingAverageByTuple(inputs.Select(inputMapper), emaPeriodCount1);
            _ema2 = new ExponentialMovingAverageByTuple(inputs.Select(inputMapper), emaPeriodCount2);
            _macd = i => _ema1[i] - _ema2[i];

            _signal = new GenericExponentialMovingAverage(
                0,
                i => _macd(i),
                i => _macd(i),
                i => 2.0m / (demPeriodCount + 1),
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
            decimal? macd = _macd(index);
            decimal? signal = _signal[index];
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

    public class MovingAverageConvergenceDivergence : MovingAverageConvergenceDivergence<Candle, AnalyzableTick<(decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram)>>
    {
        public MovingAverageConvergenceDivergence(IEnumerable<Candle> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(inputs, i => i.Close, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }
    }
}