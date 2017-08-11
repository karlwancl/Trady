using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOscillatorTrend
    {
		public class Slow<TInput, TOutput> : IndicatorBase<TInput, TOutput>
		{
			public Slow(
				IEnumerable<TInput> inputs,
				Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper,
				Func<TInput, Trend?, TOutput> outputMapper,
				int periodCount,
				int smaPeriodCountD)
				: base(inputs, inputMapper, outputMapper, new Stochastics.SlowByTuple(inputs.Select(inputMapper), periodCount, smaPeriodCountD))
			{
			}
		}

		public class SlowByTuple : Slow<(decimal High, decimal Low, decimal Close), Trend?>
		{
			public SlowByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountD)
				: base(inputs, i => i, (i, otm) => otm, periodCount, smaPeriodCountD)
			{
			}
		}

		public class Slow : Slow<Candle, AnalyzableTick<Trend?>>
		{
			public Slow(IEnumerable<Candle> inputs, int periodCount, int smaPeriodCountD)
				: base(inputs, i => (i.High, i.Low, i.Close), (i, otm) => new AnalyzableTick<Trend?>(i.DateTime, otm), periodCount, smaPeriodCountD)
			{
			}
		}
    }
}