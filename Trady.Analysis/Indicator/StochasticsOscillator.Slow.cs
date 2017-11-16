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
        public class Slow<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), TOutput>
        {
            private Stochastics.SlowByTuple _sto;

            public int PeriodCount { get; }
            public int SmaPeriodCountD { get; }

            public Slow(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount, int smaPeriodCountD) : base(inputs, inputMapper)
            {
                SmaPeriodCountD = smaPeriodCountD;
                PeriodCount = periodCount;

                _sto = new Stochastics.SlowByTuple(inputs.Select(inputMapper), periodCount, smaPeriodCountD);
            }

            protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
                => _sto[index].K - _sto[index].D;
        }

        public class SlowByTuple : Slow<(decimal High, decimal Low, decimal Close), decimal?>
        {
            public SlowByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountD) 
                : base(inputs, i => i, periodCount, smaPeriodCountD)
            {
            }
        }

        public class Slow : Slow<IOhlcv, AnalyzableTick<decimal?>>
        {
            public Slow(IEnumerable<IOhlcv> inputs, int periodCount, int smaPeriodCountD) 
                : base(inputs, i => (i.High, i.Low, i.Close), periodCount, smaPeriodCountD)
            {
            }
        }
    }
}
