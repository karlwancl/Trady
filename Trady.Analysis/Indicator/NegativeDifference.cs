using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class NegativeDifference<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        public int PeriodCount { get; }
        private DifferenceByTuple _diff;

        public NegativeDifference(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount = 1) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
            _diff = new DifferenceByTuple(inputs.Select(inputMapper), periodCount);
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
            => _diff[index].HasValue ? Math.Abs(Math.Min(_diff[index].Value, 0)) : default(decimal?);
    }

    public class NegativeDifferenceByTuple : NegativeDifference<decimal?, decimal?>
    {
        public NegativeDifferenceByTuple(IEnumerable<decimal?> inputs, int periodCount = 1)
            : base(inputs, i => i, periodCount)
        {
        }
    }
}
