using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class ClosePricePercentageChange<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public ClosePricePercentageChange(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int numberOfDays) : base(inputs, inputMapper, outputMapper)
        {
			NumberOfDays = numberOfDays;
		}

        public int NumberOfDays { get; } 

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
        => index >= NumberOfDays ? (mappedInputs.ElementAt(index) - mappedInputs.ElementAt(index - NumberOfDays)) / mappedInputs.ElementAt(index - NumberOfDays) * 100 : (decimal?)null;
	}

    public class ClosePricePercentageChangeByTuple : ClosePricePercentageChange<decimal, decimal?>
    {
        public ClosePricePercentageChangeByTuple(IEnumerable<decimal> inputs, int numberOfDays) 
            : base(inputs, i => i, (i, otm) => otm, numberOfDays)
        {
        }
    }

    public class ClosePricePercentageChange : ClosePricePercentageChange<Candle, AnalyzableTick<decimal?>>
    {
        public ClosePricePercentageChange(IEnumerable<Candle> inputs, int numberOfDays) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), numberOfDays)
        {
        }
    }
}