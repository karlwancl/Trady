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
        public class Slow<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J), TOutput>
        {
            private const int SmaPeriodCountK = 3;
            private readonly FullByTuple _fullSto;

            public Slow(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount, int smaPeriodCountD) : base(inputs, inputMapper)
            {
                _fullSto = new FullByTuple(inputs.Select(inputMapper), periodCount, SmaPeriodCountK, smaPeriodCountD);

                PeriodCount = periodCount;
                SmaPeriodCountD = smaPeriodCountD;
            }

            public int PeriodCount { get; }

            public int SmaPeriodCountD { get; }

            protected override (decimal? K, decimal? D, decimal? J) ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index) => _fullSto[index];
        }

        public class SlowByTuple : Slow<(decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J)>
        {
            public SlowByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountD)
                : base(inputs, i => i, periodCount, smaPeriodCountD)
            {
            }
        }

        public class Slow : Slow<IOhlcv, AnalyzableTick<(decimal? K, decimal? D, decimal? J)>>
        {
            public Slow(IEnumerable<IOhlcv> inputs, int periodCount, int smaPeriodCountD)
                : base(inputs, i => (i.High, i.Low, i.Close), periodCount, smaPeriodCountD)
            {
            }
        }
    }
}