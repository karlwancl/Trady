using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class EfficiencyRatio<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        public EfficiencyRatio(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
        {
            if (index <= 0 || index < PeriodCount)
                return default;

            var change = Math.Abs(mappedInputs[index] - mappedInputs[index - PeriodCount]);
            var volatility = Enumerable.Range(index - PeriodCount + 1, PeriodCount).Select(i => Math.Abs(mappedInputs[i] - mappedInputs[i - 1])).Sum();
            return volatility > 0 ? (decimal?)change / volatility : default;
        }
    }

    public class EfficiencyRatioByTuple : EfficiencyRatio<decimal, decimal?>
    {
        public EfficiencyRatioByTuple(IEnumerable<decimal> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class EfficiencyRatio : EfficiencyRatio<IOhlcv, AnalyzableTick<decimal?>>
    {
        public EfficiencyRatio(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}
