using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class BearishShortDay<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        BearishByTuple _bearish;
        ShortDayByTuple _shortDay;

        public BearishShortDay(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, Func<TInput, bool, TOutput> outputMapper, int periodCount = 20, decimal threshold = 0.25m) : base(inputs, inputMapper, outputMapper)
        {
            var ocs = inputs.Select(inputMapper);
            _bearish = new BearishByTuple(ocs);
            _shortDay = new ShortDayByTuple(ocs);

            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; private set; }

        public decimal Threshold { get; private set; }

        protected override bool ComputeByIndexImpl(IEnumerable<(decimal Open, decimal Close)> mappedInputs, int index)
			=> _bearish[index] && _shortDay[index];
	}

    public class BearishShortDayByTuple : BearishShortDay<(decimal Open, decimal Close), bool>
    {
        public BearishShortDayByTuple(IEnumerable<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25M) 
            : base(inputs, i => i, (i, otm) => otm, periodCount, threshold)
        {
        }
    }

    public class BearishShortDay : BearishShortDay<Candle, AnalyzableTick<bool>>
    {
        public BearishShortDay(IEnumerable<Candle> inputs, int periodCount = 20, decimal threshold = 0.25M) 
            : base(inputs, i => (i.Open, i.Close), (i, otm) => new AnalyzableTick<bool>(i.DateTime, otm), periodCount, threshold)
        {
        }
    }
}
