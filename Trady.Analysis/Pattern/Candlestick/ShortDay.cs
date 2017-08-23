using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class ShortDay<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
    {
        public ShortDay(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, int periodCount = 20, decimal threshold = 0.25m) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; }

        public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IEnumerable<(decimal Open, decimal Close)> mappedInputs, int index)
        {
			var bodyLengths = mappedInputs.Select(i => Math.Abs(i.Close - i.Open)).ToList();
			return bodyLengths[index] < bodyLengths.Percentile(PeriodCount, index, Threshold);
        }
    }

    public class ShortDayByTuple : ShortDay<(decimal Open, decimal Close), bool>
    {
        public ShortDayByTuple(IEnumerable<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.25M) 
            : base(inputs, i => i, periodCount, threshold)
        {
        }
    }

    public class ShortDay : ShortDay<Candle, AnalyzableTick<bool>>
    {
        public ShortDay(IEnumerable<Candle> inputs, int periodCount = 20, decimal threshold = 0.25M) 
            : base(inputs, i => (i.Open, i.Close), periodCount, threshold)
        {
        }
    }
}
