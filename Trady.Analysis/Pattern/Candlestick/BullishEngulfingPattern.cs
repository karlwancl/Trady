using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_bearish_reversal_patterns#bearish_engulfing
    /// </summary>
    public class BullishEngulfingPattern : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private DownTrend _downTrend;
        private Bearish _bearish;
        private Bullish _bullish;

        public BullishEngulfingPattern(IList<Candle> inputs, int downTrendPeriodCount = 3)
            : this(inputs.Select(c => (c.Open, c.High, c.Low, c.Close)).ToList(), downTrendPeriodCount)
        {
        }

        public BullishEngulfingPattern(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3) : base(inputs)
        {
            _downTrend = new DownTrend(inputs.Select(i => (i.High, i.Low)).ToList(), downTrendPeriodCount);

            var ocs = inputs.Select(i => (i.Open, i.Close)).ToList();
            _bearish = new Bearish(ocs);
            _bullish = new Bullish(ocs);

            DownTrendPeriodCount = downTrendPeriodCount;
        }

        public int DownTrendPeriodCount { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index < 1) return null;
            bool isEngulf = Inputs[index].Open < Inputs[index - 1].Close && Inputs[index].Close > Inputs[index - 1].Open;
            return (_downTrend[index - 1] ?? false) && _bearish[index - 1] && _bullish[index] && isEngulf;
        }
    }
}