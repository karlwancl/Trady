using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    public class BearishLongDay<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        private BearishByTuple _bearish;
        private LongDayByTuple _longDay;

        public BearishLongDay(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, int periodCount = 20, decimal threshold = 0.75m) : base(inputs, inputMapper)
        {
            _bearish = new BearishByTuple(inputs.Select(inputMapper));
            _longDay = new LongDayByTuple(inputs.Select(inputMapper));

            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }

        public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal Close)> mappedInputs, int index)
            => _bearish[index] && _longDay[index];
    }

    public class BearishLongDayByTuple : BearishLongDay<(decimal Open, decimal Close), bool>
    {
        public BearishLongDayByTuple(IEnumerable<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.75M)
            : base(inputs, i => i, periodCount, threshold)
        {
        }
    }

    public class BearishLongDay : BearishLongDay<IOhlcv, AnalyzableTick<bool>>
    {
        public BearishLongDay(IEnumerable<IOhlcv> inputs, int periodCount = 20, decimal threshold = 0.75M)
            : base(inputs, i => (i.Open, i.Close), periodCount, threshold)
        {
        }
    }
}