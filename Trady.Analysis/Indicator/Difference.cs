using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class Difference<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        public Difference(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int numberOfDays = 1) : base(inputs, inputMapper)
        {
            NumberOfDays = numberOfDays;
        }

        public int NumberOfDays { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
            => index >= NumberOfDays ? mappedInputs[index] - mappedInputs[index - NumberOfDays] : null;
    }

    public class DifferenceByTuple : Difference<decimal?, decimal?>
    {
        public DifferenceByTuple(IEnumerable<decimal?> inputs, int numberOfDays = 1)
            : base(inputs, i => i, numberOfDays)
        {
        }

		public DifferenceByTuple(IEnumerable<decimal> inputs, int numberOfDays = 1)
	        : this(inputs.Cast<decimal?>(), numberOfDays)
		{
		}
    }
}