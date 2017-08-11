using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
	/// <summary>
	/// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
	/// </summary>
	public class BullishLongDay<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal Close), bool, TOutput>
	{
        readonly BullishByTuple _bullish;
        readonly LongDayByTuple _longDay;

        public BullishLongDay(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal Close)> inputMapper, Func<TInput, bool, TOutput> outputMapper, int periodCount = 20, decimal threshold = 0.75m) : base(inputs, inputMapper, outputMapper)
        {
			_bullish = new BullishByTuple(inputs.Select(inputMapper));
            _longDay = new LongDayByTuple(inputs.Select(inputMapper), periodCount, threshold);

			PeriodCount = periodCount;
			Threshold = threshold;
        }

        public int PeriodCount { get; }

		public decimal Threshold { get; }

        protected override bool ComputeByIndexImpl(IEnumerable<(decimal Open, decimal Close)> mappedInputs, int index)
            => _bullish[index] && _longDay[index];
    }

    public class BullishLongDayByTuple : BullishLongDay<(decimal Open, decimal Close), bool>
    {
        public BullishLongDayByTuple(IEnumerable<(decimal Open, decimal Close)> inputs, int periodCount = 20, decimal threshold = 0.75M) 
            : base(inputs, i => i, (i, otm) => otm, periodCount, threshold)
        {
        }
    }

    public class BullishLongDay : BullishLongDay<Candle, AnalyzableTick<bool>>
    {
        public BullishLongDay(IEnumerable<Candle> inputs, int periodCount = 20, decimal threshold = 0.75M) 
            : base(inputs, i => (i.Open, i.Close), (i, otm) => new AnalyzableTick<bool>(i.DateTime, otm), periodCount, threshold)
        {
        }
    }
}