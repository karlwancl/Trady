using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class ClosePriceChange<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public ClosePriceChange(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount = 1) : base(inputs, inputMapper, outputMapper)
        {
			PeriodCount = periodCount;
		}

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
            => index >= PeriodCount ? mappedInputs.ElementAt(index) - mappedInputs.ElementAt(index - PeriodCount) : (decimal?)null;
	}

    public class ClosePriceChangeByTuple : ClosePriceChange<decimal, decimal?>
    {
        public ClosePriceChangeByTuple(IEnumerable<decimal> inputs, int periodCount = 1) 
            : base(inputs, i => i, (i, otm) => otm, periodCount)
        {
        }
    }

    public class ClosePriceChange : ClosePriceChange<Candle, AnalyzableTick<decimal?>>
    {
        public ClosePriceChange(IEnumerable<Candle> inputs, int periodCount = 1) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), periodCount)
        {
        }
    }
}