using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/d/downside-tasuki-gap.asp
    /// </summary>
    public class DownsideTasukiGap : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private DownTrend _downTrend;
        private Bearish _bearish;
        private Bullish _bullish;

        public DownsideTasukiGap(IList<Candle> candles, int downTrendPeriodCount = 3, decimal sizeThreshold = 0.1m)
            : this(candles.Select(c => (c.Open, c.High, c.Low, c.Close)).ToList(), downTrendPeriodCount, sizeThreshold)
        {
        }

        public DownsideTasukiGap(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, decimal sizeThreshold = 0.1m) : base(inputs)
        {
            var ocs = inputs.Select(i => (i.Open, i.Close)).ToList();
            _downTrend = new DownTrend(inputs.Select(i => (i.High, i.Low)).ToList(), downTrendPeriodCount);
            _bearish = new Bearish(ocs);
            _bullish = new Bullish(ocs);

            DownTrendPeriodCount = downTrendPeriodCount;
            SizeThreshold = sizeThreshold;
        }

        public int DownTrendPeriodCount { get; private set; }

        public decimal SizeThreshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index < 2) return null;
            bool isWhiteCandleWithinGap = Inputs[index].Close < Inputs[index - 2].Low && Inputs[index].Close > Inputs[index - 1].High;
            return (_downTrend[index - 1] ?? false) &&
                _bearish[index - 2] &&
                Inputs[index - 2].Low > Inputs[index - 1].High &&
                _bearish[index - 1] &&
                _bullish[index] &&
                isWhiteCandleWithinGap;
        }
    }
}