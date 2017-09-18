using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class Diff<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        public Diff(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int numberOfDays = 1) : base(inputs, inputMapper)
        {
            NumberOfDays = numberOfDays;
        }

        public int NumberOfDays { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => index >= NumberOfDays ? mappedInputs[index] - mappedInputs[index - NumberOfDays] : (decimal?)null;
    }

    public class DiffByTuple : Diff<decimal, decimal?>
    {
        public DiffByTuple(IEnumerable<decimal> inputs, int numberOfDays = 1)
            : base(inputs, i => i, numberOfDays)
        {
        }
    }
}