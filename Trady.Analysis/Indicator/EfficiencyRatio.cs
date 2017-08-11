using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class EfficiencyRatio<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public EfficiencyRatio(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount) : base(inputs, inputMapper, outputMapper)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
        {
			if (index <= 0 || index < PeriodCount)
				return null;

            decimal? change = Math.Abs(mappedInputs.ElementAt(index) - mappedInputs.ElementAt(index - PeriodCount));
            decimal? volatility = Enumerable.Range(index - PeriodCount + 1, PeriodCount).Select(i => Math.Abs(mappedInputs.ElementAt(i) - mappedInputs.ElementAt(i - 1))).Sum();
			return change / volatility;
        }
    }

    public class EfficiencyRatioByTuple : EfficiencyRatio<decimal, decimal?>
    {
        public EfficiencyRatioByTuple(IEnumerable<decimal> inputs, int periodCount) 
            : base(inputs, i => i, (i, otm) => otm, periodCount)
        {
        }
    }

    public class EfficiencyRatio : EfficiencyRatio<Candle, AnalyzableTick<decimal?>>
    {
        public EfficiencyRatio(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), periodCount)
        {
        }
    }
}