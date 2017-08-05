using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class LowestLow<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public LowestLow(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount) : base(inputs, inputMapper, outputMapper)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
			=> index >= PeriodCount - 1 ? mappedInputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Min() : (decimal?)null;
	}

    public class LowestLowByTuple : LowestLow<decimal, decimal?>
    {
        public LowestLowByTuple(IEnumerable<decimal> inputs, int periodCount) 
            : base(inputs, i => i, (i, otm) => otm, periodCount)
        {
        }
    }

    public class LowestLow : LowestLow<Candle, AnalyzableTick<decimal?>>
    {
        public LowestLow(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => (i.Low), (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), periodCount)
        {
        }
    }
}