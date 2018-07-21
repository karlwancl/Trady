using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_bearish_reversal_patterns#dark_cloud_cover
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class DarkCloudCover<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private UpTrendByTuple _upTrend;
        private DownTrendByTuple _downTrend;
        private BullishByTuple _bullish;
        private BearishByTuple _bearish;

        public DarkCloudCover(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int upTrendPeriodCount = 3, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75m)
            : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);

            var hls = mappedInputs.Select(i => (i.High, i.Low));
            _upTrend = new UpTrendByTuple(hls, upTrendPeriodCount);
            _downTrend = new DownTrendByTuple(hls, downTrendPeriodCount);

            var ocs = mappedInputs.Select(i => (i.Open, i.Close));
            _bullish = new BullishByTuple(ocs);
            _bearish = new BearishByTuple(ocs);

            UpTrendPeriodCount = upTrendPeriodCount;
            DownTrendPeriodCount = downTrendPeriodCount;
            LongPeriodCount = longPeriodCount;
            LongThreshold = longThreshold;
        }

        public int UpTrendPeriodCount { get; }

        public int DownTrendPeriodCount { get; }

        public int LongPeriodCount { get; }

        public decimal LongThreshold { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < DownTrendPeriodCount)
                return default;

            var i = index - DownTrendPeriodCount;
            var isPassThrough = mappedInputs[i + 1].Open > mappedInputs[i].Close && mappedInputs[i + 1].Close < (mappedInputs[i].Open + mappedInputs[i].Close) / 2;
            return (_upTrend[i] ?? false) && _bullish[i] && isPassThrough && _bearish[i + 1];
        }
    }

    public class DarkCloudCoverByTuple : DarkCloudCover<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public DarkCloudCoverByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75M)
            : base(inputs, i => i, upTrendPeriodCount, downTrendPeriodCount, longPeriodCount, longThreshold)
        {
        }
    }

    public class DarkCloudCover : DarkCloudCover<IOhlcv, AnalyzableTick<bool?>>
    {
        public DarkCloudCover(IEnumerable<IOhlcv> inputs, int upTrendPeriodCount = 3, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), upTrendPeriodCount, downTrendPeriodCount, longPeriodCount, longThreshold)
        {
        }
    }
}
