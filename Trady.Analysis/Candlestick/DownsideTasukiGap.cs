using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/d/downside-tasuki-gap.asp
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class DownsideTasukiGap<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private DownTrendByTuple _downTrend;
        private BearishByTuple _bearish;
        private BullishByTuple _bullish;

        public DownsideTasukiGap(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int downTrendPeriodCount = 3, decimal sizeThreshold = 0.1m) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);

            var ocs = mappedInputs.Select(i => (i.Open, i.Close));
            _downTrend = new DownTrendByTuple(mappedInputs.Select(i => (i.High, i.Low)), downTrendPeriodCount);
            _bearish = new BearishByTuple(ocs);
            _bullish = new BullishByTuple(ocs);

            DownTrendPeriodCount = downTrendPeriodCount;
            SizeThreshold = sizeThreshold;
        }

        public int DownTrendPeriodCount { get; }

        public decimal SizeThreshold { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < 2)
                return default;

            var isWhiteIOhlcvDataWithinGap = mappedInputs[index].Close < mappedInputs[index - 2].Low && mappedInputs[index].Close > mappedInputs[index - 1].High;
            return (_downTrend[index - 1] ?? false) &&
                _bearish[index - 2] &&
                mappedInputs[index - 2].Low > mappedInputs[index - 1].High &&
                _bearish[index - 1] &&
                _bullish[index] &&
                isWhiteIOhlcvDataWithinGap;
        }
    }

    public class DownsideTasukiGapByTuple : DownsideTasukiGap<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public DownsideTasukiGapByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, decimal sizeThreshold = 0.1M)
            : base(inputs, i => i, downTrendPeriodCount, sizeThreshold)
        {
        }
    }

    public class DownsideTasukiGap : DownsideTasukiGap<IOhlcv, AnalyzableTick<bool?>>
    {
        public DownsideTasukiGap(IEnumerable<IOhlcv> inputs, int downTrendPeriodCount = 3, decimal sizeThreshold = 0.1M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), downTrendPeriodCount, sizeThreshold)
        {
        }
    }
}
