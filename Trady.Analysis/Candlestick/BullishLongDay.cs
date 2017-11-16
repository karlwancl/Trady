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
    public class BullishLongDay<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        private readonly BullishByTuple _bullish;
        private readonly LongDayByTuple _longDay;

        public BullishLongDay(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, int periodCount = 20, decimal threshold = 0.75m) : base(inputs, inputMapper)
        {
            _bullish = new BullishByTuple(inputs.Select(inputMapper));
            _longDay = new LongDayByTuple(inputs.Select(inputMapper), periodCount, threshold);

            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }

        public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal Close)> mappedInputs, int index)
            => _bullish[index] && _longDay[index];
    }

    public class BullishLongDayByTuple : BullishLongDay<(decimal Open, decimal Close), bool>
    {
        public BullishLongDayByTuple(IEnumerable<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.75M)
            : base(inputs, i => i, periodCount, threshold)
        {
        }
    }

    public class BullishLongDay : BullishLongDay<IOhlcv, AnalyzableTick<bool>>
    {
        public BullishLongDay(IEnumerable<IOhlcv> inputs, int periodCount = 20, decimal threshold = 0.75M)
            : base(inputs, i => (i.Open, i.Close), periodCount, threshold)
        {
        }
    }
}