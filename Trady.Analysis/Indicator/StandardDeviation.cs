using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class StandardDeviation<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public StandardDeviation(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index) 
        {
			if (index < PeriodCount - 1)
				return null;

            var subset = mappedInputs.Skip(index - PeriodCount + 1).Take(PeriodCount);
            decimal average = subset.Average();
            decimal sumOfDiff = subset.Select(v => (v - average) * (v - average)).Sum();
			return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(sumOfDiff / (subset.Count() - 1))));
        }
    }

    public class StandardDeviationByTuple : StandardDeviation<decimal, decimal?>
    {
        public StandardDeviationByTuple(IEnumerable<decimal> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class StandardDeviation : StandardDeviation<Candle, AnalyzableTick<decimal?>>
    {
        public StandardDeviation(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}