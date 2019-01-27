using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_bearish_reversal_patterns#bearish_engulfing
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class BearishEngulfingPattern<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private UpTrendByTuple _upTrend;
        private BullishByTuple _bullish;
        private BearishByTuple _bearish;

        public BearishEngulfingPattern(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int upTrendPeriodCount = 3) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            _upTrend = new UpTrendByTuple(mappedInputs.Select(i => (i.High, i.Low)), upTrendPeriodCount);

            var ocs = mappedInputs.Select(i => (i.Open, i.Close));
            _bullish = new BullishByTuple(ocs);
            _bearish = new BearishByTuple(ocs);

            UpTrendPeriodCount = upTrendPeriodCount;
        }

        public int UpTrendPeriodCount { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < 1)
                return default;

            var isEngulf = mappedInputs[index].Open > mappedInputs[index - 1].Close && mappedInputs[index].Close < mappedInputs[index - 1].Open;
            return (_upTrend[index - 1] ?? false) && _bullish[index - 1] && _bearish[index] && isEngulf;
        }
    }

    public class BearishEngulfingPatternByTuple : BearishEngulfingPattern<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public BearishEngulfingPatternByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3)
            : base(inputs, i => i, upTrendPeriodCount)
        {
        }
    }

    public class BearishEngulfingPattern : BearishEngulfingPattern<IOhlcv, AnalyzableTick<bool?>>
    {
        public BearishEngulfingPattern(IEnumerable<IOhlcv> inputs, int upTrendPeriodCount = 3)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), upTrendPeriodCount)
        {
        }
    }
}
