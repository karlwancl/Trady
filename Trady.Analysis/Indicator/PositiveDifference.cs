using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class PositiveDifference<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        public int PeriodCount { get; }
        private DifferenceByTuple _diff;

        public PositiveDifference(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount = 1) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
            _diff = new DifferenceByTuple(inputs.Select(inputMapper), periodCount);
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
            => _diff[index].HasValue ? Math.Max(_diff[index].Value, 0) : default(decimal?);
    }

    public class PositiveDifferenceByTuple : PositiveDifference<decimal?, decimal?>
    {
        public PositiveDifferenceByTuple(IEnumerable<decimal?> inputs, int periodCount = 1) 
            : base(inputs, i => i, periodCount)
        {
        }
    }
}
