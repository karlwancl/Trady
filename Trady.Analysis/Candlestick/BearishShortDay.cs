using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    public class BearishShortDay<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        private BearishByTuple _bearish;
        private ShortDayByTuple _shortDay;

        public BearishShortDay(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, int periodCount = 20, decimal threshold = 0.25m) : base(inputs, inputMapper)
        {
            var ocs = inputs.Select(inputMapper);
            _bearish = new BearishByTuple(ocs);
            _shortDay = new ShortDayByTuple(ocs);

            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }

        public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal Close)> mappedInputs, int index)
            => _bearish[index] && _shortDay[index];
    }

    public class BearishShortDayByTuple : BearishShortDay<(decimal Open, decimal Close), bool>
    {
        public BearishShortDayByTuple(IEnumerable<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25M)
            : base(inputs, i => i, periodCount, threshold)
        {
        }
    }

    public class BearishShortDay : BearishShortDay<IOhlcv, AnalyzableTick<bool>>
    {
        public BearishShortDay(IEnumerable<IOhlcv> inputs, int periodCount = 20, decimal threshold = 0.25M)
            : base(inputs, i => (i.Open, i.Close), periodCount, threshold)
        {
        }
    }
}