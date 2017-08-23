using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class HighestHigh<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public HighestHigh(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
            => index >= PeriodCount - 1 ? mappedInputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Max() : (decimal?)null;
    }

    public class HighestHighByTuple : HighestHigh<decimal, decimal?>
    {
        public HighestHighByTuple(IEnumerable<decimal> inputs,int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class HighestHigh : HighestHigh<Candle, AnalyzableTick<decimal?>>
    {
        public HighestHigh(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => i.High, periodCount)
        {
        }
    }
}