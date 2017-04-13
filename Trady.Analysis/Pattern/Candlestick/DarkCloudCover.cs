using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_bearish_reversal_patterns#dark_cloud_cover
    /// </summary>
    public class DarkCloudCover : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private UpTrend _upTrend;
        private DownTrend _downTrend;
        private Bullish _bullish;
        private Bearish _bearish;

        public DarkCloudCover(IList<Candle> inputs, int upTrendPeriodCount = 3, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75m) 
            : this(inputs.Select(i => (i.Open, i.High, i.Low, i.Close)).ToList(), upTrendPeriodCount, downTrendPeriodCount, longPeriodCount, longThreshold)
        {
        }

        public DarkCloudCover(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75m) : base(inputs)
        {
            var hls = inputs.Select(i => (i.High, i.Low)).ToList();
            _upTrend = new UpTrend(hls, upTrendPeriodCount);
            _downTrend = new DownTrend(hls, downTrendPeriodCount);

            var ocs = inputs.Select(i => (i.Open, i.Close)).ToList();
            _bullish = new Bullish(ocs);
            _bearish = new Bearish(ocs);

            UpTrendPeriodCount = upTrendPeriodCount;
            DownTrendPeriodCount = downTrendPeriodCount;
            LongPeriodCount = longPeriodCount;
            LongThreshold = longThreshold;
        }

        public int UpTrendPeriodCount { get; private set; }

        public int DownTrendPeriodCount { get; private set; }

        public int LongPeriodCount { get; private set; }

        public decimal LongThreshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index < DownTrendPeriodCount) return null;
            int i = index - DownTrendPeriodCount;
            bool isPassThrough = Inputs[i + 1].Open > Inputs[i].Close && Inputs[i + 1].Close < (Inputs[i].Open + Inputs[i].Close) / 2;
            return (_upTrend[i] ?? false) && _bullish[i] && isPassThrough && _bearish[i + 1];
        }
    }
}