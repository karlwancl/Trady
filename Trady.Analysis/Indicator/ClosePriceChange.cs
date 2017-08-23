using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class ClosePriceChange<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public ClosePriceChange(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount = 1) : base(inputs, inputMapper)
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
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class ClosePriceChange : ClosePriceChange<Candle, AnalyzableTick<decimal?>>
    {
        public ClosePriceChange(IEnumerable<Candle> inputs, int periodCount = 1) 
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}