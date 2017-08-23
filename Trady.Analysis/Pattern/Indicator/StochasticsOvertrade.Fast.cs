using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOvertrade
    {
        public class Fast<TInput, TOutput> : IndicatorBase<TInput, TOutput>
        {
            protected Fast(
                IEnumerable<TInput> inputs, 
                Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, 
                int periodCount,
                int smaPeriodCount) 
                : base(inputs, inputMapper, new Stochastics.FastByTuple(inputs.Select(inputMapper), periodCount, smaPeriodCount))
            {
            }
        }

        public class FastByTuple : Fast<(decimal High, decimal Low, decimal Close), Overtrade?>
        {
            public FastByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCount) 
                : base(inputs, i => i, periodCount, smaPeriodCount)
            {
            }
        }

        public class Fast : Fast<Candle, AnalyzableTick<Overtrade?>>
        {
            public Fast(IEnumerable<Candle> inputs, int periodCount, int smaPeriodCount) 
                : base(inputs, i => (i.High, i.Low, i.Close), periodCount, smaPeriodCount)
            {
            }
        }
    }
}