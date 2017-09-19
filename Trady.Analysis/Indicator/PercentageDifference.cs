using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
	public class PercentageDifference<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
	{
        public PercentageDifference(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int numberOfDays = 1) : base(inputs, inputMapper)
		{
            NumberOfDays = numberOfDays;
		}

        public int NumberOfDays { get; }

		protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
            => index >= NumberOfDays ? (mappedInputs[index] - mappedInputs[index - NumberOfDays]) / mappedInputs[index - NumberOfDays] * 100 : (decimal?)null;
	}

	public class PercentageDifferenceByTuple : PercentageDifference<decimal?, decimal?>
	{
        public PercentageDifferenceByTuple(IEnumerable<decimal?> inputs, int numberOfDays = 1)
			: base(inputs, i => i, numberOfDays)
		{
		}

        public PercentageDifferenceByTuple(IEnumerable<decimal> inputs, int numberOfDays = 1)
	        : this(inputs.Cast<decimal?>(), numberOfDays)
		{
		}
	}
}
