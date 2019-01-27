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
    public class BullishEngulfingPattern<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private DownTrendByTuple _downTrend;
        private BearishByTuple _bearish;
        private BullishByTuple _bullish;

        public BullishEngulfingPattern(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int downTrendPeriodCount = 3) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            _downTrend = new DownTrendByTuple(mappedInputs.Select(i => (i.High, i.Low)), downTrendPeriodCount);

            var ocs = mappedInputs.Select(i => (i.Open, i.Close));
            _bearish = new BearishByTuple(ocs);
            _bullish = new BullishByTuple(ocs);

            DownTrendPeriodCount = downTrendPeriodCount;
        }

        public int DownTrendPeriodCount { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < 1)
                return default;

            var isEngulf = mappedInputs[index].Open < mappedInputs[index - 1].Close && mappedInputs[index].Close > mappedInputs[index - 1].Open;
            return (_downTrend[index - 1] ?? false) && _bearish[index - 1] && _bullish[index] && isEngulf;
        }
    }

    public class BullishEngulfingPatternByTuple : BullishEngulfingPattern<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public BullishEngulfingPatternByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3)
            : base(inputs, i => i, downTrendPeriodCount)
        {
        }
    }

    public class BullishEngulfingPattern : BullishEngulfingPattern<IOhlcv, AnalyzableTick<bool?>>
    {
        public BullishEngulfingPattern(IEnumerable<IOhlcv> inputs, int downTrendPeriodCount = 3)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), downTrendPeriodCount)
        {
        }
    }
}
