using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class Full<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J), TOutput>
        {
            private readonly FastByTuple _fastSto;

            public Full(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount, int smaPeriodCountK, int smaPeriodCountD) : base(inputs, inputMapper)
            {
                _fastSto = new FastByTuple(inputs.Select(inputMapper), periodCount, smaPeriodCountK);

                PeriodCount = periodCount;
                SmaPeriodCountK = smaPeriodCountK;
                SmaPeriodCountD = smaPeriodCountD;
            }

            public int PeriodCount { get; }

            public int SmaPeriodCountK { get; }

            public int SmaPeriodCountD { get; }

            protected override (decimal? K, decimal? D, decimal? J) ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
            {
                decimal? dFunc(int i) => _fastSto[i].D;
                var dAvg = index >= SmaPeriodCountD - 1 ? Enumerable.Range(index - SmaPeriodCountD + 1, SmaPeriodCountD).Average(i => dFunc(i)) : default;
				return (dFunc(index), dAvg, 3 * dFunc(index) - 2 * dAvg);
            }
        }

        public class FullByTuple : Full<(decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J)>
        {
            public FullByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : base(inputs, i => i, periodCount, smaPeriodCountK, smaPeriodCountD)
            {
            }
        }

        public class Full : Full<IOhlcv, AnalyzableTick<(decimal? K, decimal? D, decimal? J)>>
        {
            public Full(IEnumerable<IOhlcv> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : base(inputs, i => (i.High, i.Low, i.Close), periodCount, smaPeriodCountK, smaPeriodCountD)
            {
            }
        }
    }
}
