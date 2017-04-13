using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_bearish_reversal_patterns#bearish_engulfing
    /// </summary>
    public class BearishEngulfingPattern : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private UpTrend _upTrend;
        private Bullish _bullish;
        private Bearish _bearish;

        public BearishEngulfingPattern(IList<Candle> inputs, int upTrendPeriodCount = 3)
            : this(inputs.Select(c => (c.Open, c.High, c.Low, c.Close)).ToList(), upTrendPeriodCount)
        {
        }

        public BearishEngulfingPattern(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3) : base(inputs)
        {
            _upTrend = new UpTrend(inputs.Select(i => (i.High, i.Low)).ToList(), upTrendPeriodCount);

            var ocs = inputs.Select(i => (i.Open, i.Close)).ToList();
            _bullish = new Bullish(ocs);
            _bearish = new Bearish(ocs);

            UpTrendPeriodCount = upTrendPeriodCount;
        }

        public int UpTrendPeriodCount { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index < 1) return null;
            bool isEngulf = Inputs[index].Open > Inputs[index - 1].Close && Inputs[index].Close < Inputs[index - 1].Open;
            return (_upTrend[index - 1] ?? false) && _bullish[index - 1] && _bearish[index] && isEngulf;
        }
    }
}
