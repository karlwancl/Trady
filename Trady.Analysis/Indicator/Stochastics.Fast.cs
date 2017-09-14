using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class Fast<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J), TOutput>
        {
            private readonly RawStochasticsValueByTuple _rsv;

            public Fast(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount, int smaPeriodCount) : base(inputs, inputMapper)
            {
                _rsv = new RawStochasticsValueByTuple(inputs.Select(inputMapper), periodCount);

                PeriodCount = periodCount;
                SmaPeriodCount = smaPeriodCount;
            }

            public int PeriodCount { get; }

            public int SmaPeriodCount { get; }

            protected override (decimal? K, decimal? D, decimal? J) ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
            {
                decimal? rsv = _rsv[index];
                Func<int, decimal?> rsvFunc = i => _rsv[index - SmaPeriodCount + i + 1];
                decimal? rsvAvg = index >= SmaPeriodCount - 1 ? Enumerable.Range(0, SmaPeriodCount).Average(i => rsvFunc(i)) : null;
                return (rsv, rsvAvg, 3 * rsv - 2 * rsvAvg);
            }
        }

        public class FastByTuple : Fast<(decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J)>
        {
            public FastByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCount)
                : base(inputs, i => i, periodCount, smaPeriodCount)
            {
            }
        }

        public class Fast : Fast<Candle, AnalyzableTick<(decimal? K, decimal? D, decimal? J)>>
        {
            public Fast(IEnumerable<Candle> inputs, int periodCount, int smaPeriodCount)
                : base(inputs, i => (i.High, i.Low, i.Close), periodCount, smaPeriodCount)
            {
            }
        }
    }
}