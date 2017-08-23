using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class ClosePricePercentageChange<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public ClosePricePercentageChange(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int numberOfDays = 1) 
            : base(inputs, inputMapper)
        {
			NumberOfDays = numberOfDays;
		}

        public int NumberOfDays { get; } 

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
        => index >= NumberOfDays ? (mappedInputs.ElementAt(index) - mappedInputs.ElementAt(index - NumberOfDays)) / mappedInputs.ElementAt(index - NumberOfDays) * 100 : (decimal?)null;
	}

    public class ClosePricePercentageChangeByTuple : ClosePricePercentageChange<decimal, decimal?>
    {
        public ClosePricePercentageChangeByTuple(IEnumerable<decimal> inputs, int numberOfDays = 1) 
            : base(inputs, i => i, numberOfDays)
        {
        }
    }

    public class ClosePricePercentageChange : ClosePricePercentageChange<Candle, AnalyzableTick<decimal?>>
    {
        public ClosePricePercentageChange(IEnumerable<Candle> inputs, int numberOfDays = 1) 
            : base(inputs, i => i.Close, numberOfDays)
        {
        }
    }
}