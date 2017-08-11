using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class BearishLongDay<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        BearishByTuple _bearish;
        LongDayByTuple _longDay;

        public BearishLongDay(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, Func<TInput, bool, TOutput> outputMapper, int periodCount = 20, decimal threshold = 0.75m) : base(inputs, inputMapper, outputMapper)
        {
            _bearish = new BearishByTuple(inputs.Select(inputMapper));
            _longDay = new LongDayByTuple(inputs.Select(inputMapper));

            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }

        public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IEnumerable<(decimal Open, decimal Close)> mappedInputs, int index)
			=> _bearish[index] && _longDay[index];
	}

    public class BearishLongDayByTuple : BearishLongDay<(decimal Open, decimal Close), bool>
    {
        public BearishLongDayByTuple(IEnumerable<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.75M)
            : base(inputs, i => i, (i, otm) => otm, periodCount, threshold)
        {
        }
    }

    public class BearishLongDay : BearishLongDay<Candle, AnalyzableTick<bool>>
    {
        public BearishLongDay(IEnumerable<Candle> inputs, int periodCount = 20, decimal threshold = 0.75M) 
            : base(inputs, i => (i.Open, i.Close), (i, otm) => new AnalyzableTick<bool>(i.DateTime, otm), periodCount, threshold)
        {
        }
    }
}
