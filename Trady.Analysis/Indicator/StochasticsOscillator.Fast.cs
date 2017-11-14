using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public partial class StochasticsOscillator
    {
        public class Fast<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), TOutput>
        {
            private Stochastics.FastByTuple _sto;

            public int PeriodCount { get; }
            public int SmaPeriodCount { get; }

            public Fast(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount, int smaPeriodCount) : base(inputs, inputMapper)
            {
                _sto = new Stochastics.FastByTuple(inputs.Select(inputMapper), periodCount, smaPeriodCount);

                SmaPeriodCount = smaPeriodCount;
                PeriodCount = periodCount;
            }

            protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
                => _sto[index].K - _sto[index].D;
        }

        public class FastByTuple : Fast<(decimal High, decimal Low, decimal Close), decimal?>
        {
            public FastByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCount) 
                : base(inputs, i => i, periodCount, smaPeriodCount)
            {
            }
        }

        public class Fast : Fast<IOhlcv, AnalyzableTick<decimal?>>
        {
            public Fast(IEnumerable<IOhlcv> inputs, int periodCount, int smaPeriodCount) 
                : base(inputs, i => (i.High, i.Low, i.Close), periodCount, smaPeriodCount)
            {
            }
        }
    }
}
