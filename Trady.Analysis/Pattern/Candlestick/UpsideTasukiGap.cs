using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/u/upside-tasuki-gap.asp
    /// </summary>
    public class UpsideTasukiGap : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private UpTrend _upTrend;
        private Bearish _bearish;
        private Bullish _bullish;

        public UpsideTasukiGap(IList<Candle> candles, int upTrendPeriodCount = 3, decimal sizeThreshold = 0.1m)
            : this(candles.Select(c => (c.Open, c.High, c.Low, c.Close)).ToList(), upTrendPeriodCount, sizeThreshold)
        {
        }

        public UpsideTasukiGap(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, decimal sizeThreshold = 0.1m) : base(inputs)
        {
            var ocs = inputs.Select(i => (i.Open, i.Close)).ToList();
            _upTrend = new UpTrend(inputs.Select(i => (i.High, i.Low)).ToList(), upTrendPeriodCount);
            _bearish = new Bearish(ocs);
            _bullish = new Bullish(ocs);

            UpTrendPeriodCount = upTrendPeriodCount;
            SizeThreshold = sizeThreshold;
        }

        public int UpTrendPeriodCount { get; private set; }

        public decimal SizeThreshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index < 2) return null;
            bool isBlackCandleWithinGap = Inputs[index].Open > Inputs[index - 1].Open && Inputs[index].Open < Inputs[index - 1].Close && Inputs[index].Close < Inputs[index - 1].Open;
            return (_upTrend[index - 1] ?? false) &&
                _bullish[index - 2] &&
                Inputs[index - 2].High < Inputs[index - 1].Low &&
                _bullish[index - 1] &&
                _bearish[index] &&
                isBlackCandleWithinGap;
        }
    }
}