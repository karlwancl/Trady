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
		public class Full<TInput, TOutput> : IndicatorBase<TInput, TOutput>
		{
			protected Full(
				IEnumerable<TInput> inputs,
				Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper,
				Func<TInput, Overtrade?, TOutput> outputMapper,
				int periodCount,
				int smaPeriodCountK,
                int smaPeriodCountD)
				: base(inputs, inputMapper, outputMapper, new Stochastics.FullByTuple(inputs.Select(inputMapper), periodCount, smaPeriodCountK, smaPeriodCountD))
			{
			}
		}

		public class FullByTuple : Full<(decimal High, decimal Low, decimal Close), Overtrade?>
		{
			public FullByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
				: base(inputs, i => i, (i, otm) => otm, periodCount, smaPeriodCountK, smaPeriodCountD)
			{
			}
		}

		public class Full : Full<Candle, AnalyzableTick<Overtrade?>>
		{
			public Full(IEnumerable<Candle> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
				: base(inputs, i => (i.High, i.Low, i.Close), (i, otm) => new AnalyzableTick<Overtrade?>(i.DateTime, otm), periodCount, smaPeriodCountK, smaPeriodCountD)
			{
			}
		}
    }
}