using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class ClosePriceChange<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public ClosePriceChange(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int numberOfDays) : base(inputs, inputMapper, outputMapper)
        {
			NumberOfDays = numberOfDays;
		}

        public int NumberOfDays { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
            => index >= NumberOfDays ? mappedInputs.ElementAt(index) - mappedInputs.ElementAt(index - NumberOfDays) : (decimal?)null;
	}

    public class ClosePriceChangeByTuple : ClosePriceChange<decimal, decimal?>
    {
        public ClosePriceChangeByTuple(IEnumerable<decimal> inputs, int numberOfDays) 
            : base(inputs, i => i, (i, otm) => otm, numberOfDays)
        {
        }
    }

    public class ClosePriceChange : ClosePriceChange<Candle, AnalyzableTick<decimal?>>
    {
        public ClosePriceChange(IEnumerable<Candle> inputs, int numberOfDays) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), numberOfDays)
        {
        }
    }
}