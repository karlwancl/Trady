using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/u/upside-tasuki-gap.asp
    /// </summary>
    public class UpsideTasukiGap<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private readonly UpTrendByTuple _upTrend;
        private readonly BearishByTuple _bearish;
        private readonly BullishByTuple _bullish;

        public UpsideTasukiGap(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int upTrendPeriodCount = 3, decimal sizeThreshold = 0.1m) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);

            var ocs = mappedInputs.Select(i => (i.Open, i.Close));
            _upTrend = new UpTrendByTuple(mappedInputs.Select(i => (i.High, i.Low)), upTrendPeriodCount);
            _bearish = new BearishByTuple(ocs);
            _bullish = new BullishByTuple(ocs);

            UpTrendPeriodCount = upTrendPeriodCount;
            SizeThreshold = sizeThreshold;
        }

        public int UpTrendPeriodCount { get; }

        public decimal SizeThreshold { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < 2) return default;

            bool isBlackIOhlcvDataWithinGap = (mappedInputs[index].Open > mappedInputs[index - 1].Open) &&
                (mappedInputs[index].Open < mappedInputs[index - 1].Close) &&
                (mappedInputs[index].Close < mappedInputs[index - 1].Open);

            return (_upTrend[index - 1] ?? false) &&
                _bullish[index - 2] &&
                mappedInputs[index - 2].High < mappedInputs[index - 1].Low &&
                _bullish[index - 1] &&
                _bearish[index] &&
                isBlackIOhlcvDataWithinGap;
        }
    }

    public class UpsideTasukiGapByTuple : UpsideTasukiGap<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public UpsideTasukiGapByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, decimal sizeThreshold = 0.1M)
            : base(inputs, i => i, upTrendPeriodCount, sizeThreshold)
        {
        }
    }

    public class UpsideTasukiGap : UpsideTasukiGap<IOhlcv, AnalyzableTick<bool?>>
    {
        public UpsideTasukiGap(IEnumerable<IOhlcv> inputs, int upTrendPeriodCount = 3, decimal sizeThreshold = 0.1M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), upTrendPeriodCount, sizeThreshold)
        {
        }
    }
}
