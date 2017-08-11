using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class LongDay<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        public LongDay(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, Func<TInput, bool, TOutput> outputMapper, int periodCount = 20, decimal threshold = 0.75m) : base(inputs, inputMapper, outputMapper)
        {
            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }

        public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IEnumerable<(decimal Open, decimal Close)> mappedInputs, int index)
        {
            var bodyLengths = mappedInputs.Select(i => Math.Abs(i.Close - i.Open));
			return bodyLengths.ElementAt(index) >= bodyLengths.Percentile(PeriodCount, index, Threshold);
        }
    }

    public class LongDayByTuple : LongDay<(decimal Open, decimal Close), bool>
    {
        public LongDayByTuple(IEnumerable<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.75M) 
            : base(inputs, i => i, (i, otm) => otm, periodCount, threshold)
        {
        }
    }

    public class LongDay : LongDay<Candle, AnalyzableTick<bool>>
    {
        public LongDay(IEnumerable<Candle> inputs, int periodCount = 20, decimal threshold = 0.75M) 
            : base(inputs, i => (i.Open, i.Close), (i, otm) => new AnalyzableTick<bool>(i.DateTime, otm), periodCount, threshold)
        {
        }
    }
}
