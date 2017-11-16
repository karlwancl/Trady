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
        public class Full<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), TOutput>
        {
            private Stochastics.FullByTuple _sto;

            public int PeriodCount { get; }
            public int SmaPeriodCountK { get; }
            public int SmaPeriodCountD { get; }

            public Full(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount, int smaPeriodCountK, int smaPeriodCountD) : base(inputs, inputMapper)
            {
                SmaPeriodCountD = smaPeriodCountD;
                SmaPeriodCountK = smaPeriodCountK;
                PeriodCount = periodCount;

                _sto = new Stochastics.FullByTuple(inputs.Select(inputMapper), periodCount, smaPeriodCountK, smaPeriodCountD);
            }

            protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
                => _sto[index].K - _sto[index].D;
        }

        public class FullByTuple : Full<(decimal High, decimal Low, decimal Close), decimal?>
        {
            public FullByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD) 
                : base(inputs, i => i, periodCount, smaPeriodCountK, smaPeriodCountD)
            {
            }
        }

        public class Full : Full<IOhlcv, AnalyzableTick<decimal?>>
        {
            public Full(IEnumerable<IOhlcv> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD) 
                : base(inputs, i => (i.High, i.Low, i.Close), periodCount, smaPeriodCountK, smaPeriodCountD)
            {
            }
        }
    }
}
