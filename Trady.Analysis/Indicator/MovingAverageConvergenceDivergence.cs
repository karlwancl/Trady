using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class MovingAverageConvergenceDivergence : AnalyzableBase<decimal, (decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram)>
    {
        private ExponentialMovingAverage _ema1, _ema2;
        private GenericExponentialMovingAverage<decimal> _signal;
        private Func<int, decimal?> _macd;

        public MovingAverageConvergenceDivergence(IList<Candle> candles, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : this(candles.Select(c => c.Close).ToList(), emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }

        public MovingAverageConvergenceDivergence(IList<decimal> closes, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(closes)
        {
            _ema1 = new ExponentialMovingAverage(closes, emaPeriodCount1);
            _ema2 = new ExponentialMovingAverage(closes, emaPeriodCount2);
            _macd = i => _ema1[i] - _ema2[i];

            _signal = new GenericExponentialMovingAverage<decimal>(
                closes,
                0,
                i => _macd(i),
                i => _macd(i),
                i => 2.0m / (demPeriodCount + 1));

            EmaPeriodCount1 = emaPeriodCount1;
            EmaPeriodCount2 = emaPeriodCount2;
            DemPeriodCount = demPeriodCount;
        }

        public int EmaPeriodCount1 { get; private set; }

        public int EmaPeriodCount2 { get; private set; }

        public int DemPeriodCount { get; private set; }

        protected override (decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram) ComputeByIndexImpl(int index)
        {
            decimal? macd = _macd(index);
            decimal? signal = _signal[index];
            return (macd, signal, macd - signal);
        }
    }
}