using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class StandardDeviation<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        public StandardDeviation(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index) 
        {
			if (index < PeriodCount - 1)
				return null;

            var subset = mappedInputs.Skip(index - PeriodCount + 1).Take(PeriodCount);
            var average = subset.Average();
            var sumOfDiff = subset.Select(v => (v - average) * (v - average)).Sum();

            if (!sumOfDiff.HasValue)
                return default;
            
			return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(sumOfDiff / subset.Count())));
        }
    }

    public class StandardDeviationByTuple : StandardDeviation<decimal?, decimal?>
    {
        public StandardDeviationByTuple(IEnumerable<decimal?> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }

		public StandardDeviationByTuple(IEnumerable<decimal> inputs, int periodCount)
	        : this(inputs.Cast<decimal?>(), periodCount)
		{
		}
    }

    public class StandardDeviation : StandardDeviation<IOhlcv, AnalyzableTick<decimal?>>
    {
        public StandardDeviation(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}
