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
    public class UpsideTasukiGap<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        readonly UpTrendByTuple _upTrend;
        readonly BearishByTuple _bearish;
        readonly BullishByTuple _bullish;

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

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < 2) return null;
            bool isBlackCandleWithinGap = (mappedInputs.ElementAt(index).Open > mappedInputs.ElementAt(index - 1).Open) &&
                (mappedInputs.ElementAt(index).Open < mappedInputs.ElementAt(index - 1).Close) &&
                (mappedInputs.ElementAt(index).Close < mappedInputs.ElementAt(index - 1).Open);
            return (_upTrend[index - 1] ?? false) &&
                _bullish[index - 2] &&
                mappedInputs.ElementAt(index - 2).High < mappedInputs.ElementAt(index - 1).Low &&
                _bullish[index - 1] &&
                _bearish[index] &&
                isBlackCandleWithinGap;
        }
    }

    public class UpsideTasukiGapByTuple : UpsideTasukiGap<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public UpsideTasukiGapByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, decimal sizeThreshold = 0.1M) 
            : base(inputs, i => i, upTrendPeriodCount, sizeThreshold)
        {
        }
    }

    public class UpsideTasukiGap : UpsideTasukiGap<Candle, AnalyzableTick<bool?>>
    {
        public UpsideTasukiGap(IEnumerable<Candle> inputs, int upTrendPeriodCount = 3, decimal sizeThreshold = 0.1M) 
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), upTrendPeriodCount, sizeThreshold)
        {
        }
    }
}