using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class StandardDeviation<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public StandardDeviation(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount) : base(inputs, inputMapper, outputMapper)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index) => mappedInputs.SdInt(PeriodCount, index);
    }

    public class StandardDeviationByTuple : StandardDeviation<decimal, decimal?>
    {
        public StandardDeviationByTuple(IEnumerable<decimal> inputs, int periodCount) 
            : base(inputs, i => i, (i, otm) => otm, periodCount)
        {
        }
    }

    public class StandardDeviation : StandardDeviation<Candle, AnalyzableTick<decimal?>>
    {
        public StandardDeviation(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), periodCount)
        {
        }
    }
}