using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class BullishShortDay<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        private BullishByTuple _bullish;
        private ShortDayByTuple _shortDay;

        public BullishShortDay(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, int periodCount = 20, decimal threshold = 0.25m) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            _bullish = new BullishByTuple(mappedInputs);
            _shortDay = new ShortDayByTuple(mappedInputs);

            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }

        public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal Close)> mappedInputs, int index)
            => _bullish[index] && _shortDay[index];
    }

    public class BullishShortDayByTuple : BullishShortDay<(decimal Open, decimal Close), bool>
    {
        public BullishShortDayByTuple(IEnumerable<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25M)
            : base(inputs, i => i, periodCount, threshold)
        {
        }
    }

    public class BullishShortDay : BullishShortDay<IOhlcv, AnalyzableTick<bool>>
    {
        public BullishShortDay(IEnumerable<IOhlcv> inputs, int periodCount = 20, decimal threshold = 0.25M)
            : base(inputs, i => (i.Open, i.Close), periodCount, threshold)
        {
        }
    }
}
