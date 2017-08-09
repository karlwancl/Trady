using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_bearish_reversal_patterns#bearish_abandoned_baby
    /// </summary>
    public class BullishAbandonedBaby<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        DownTrendByTuple _downTrend;
        BearishLongDayByTuple _bearishLongDay;
        BullishLongDayByTuple _bullishLongDay;
        DojiByTuple _doji;

        public BullishAbandonedBaby(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, Func<TInput, bool?, TOutput> outputMapper, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75m, decimal dojiThreshold = 0.1m) : base(inputs, inputMapper, outputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            var ocs = mappedInputs.Select(i => (i.Open, i.Close));

            _downTrend = new DownTrendByTuple(mappedInputs.Select(i => (i.High, i.Low)), downTrendPeriodCount);
            _bearishLongDay = new BearishLongDayByTuple(ocs, longPeriodCount, longThreshold);
            _bullishLongDay = new BullishLongDayByTuple(ocs, longPeriodCount, longThreshold);
            _doji = new DojiByTuple(mappedInputs, dojiThreshold);

            DownTrendPeriodCount = downTrendPeriodCount;
            LongPeriodCount = longPeriodCount;
            LongThreshold = longThreshold;
            DojiThreshold = dojiThreshold;
        }

        public int DownTrendPeriodCount { get; }

        public int LongPeriodCount { get; }

        public decimal LongThreshold { get; }

        public decimal DojiThreshold { get; }

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
			if (index < 2) return null;
			if (!_doji[index - 1]) return false;
            bool isGapped = mappedInputs.ElementAt(index - 1).High < mappedInputs.ElementAt(index - 2).Low && mappedInputs.ElementAt(index - 1).High < mappedInputs.ElementAt(index).Low;
			return (_downTrend[index - 1] ?? false) && _bearishLongDay[index - 2] && isGapped && _bullishLongDay[index];
        }
    }

    public class BullishAbandonedBabyByTuple : BullishAbandonedBaby<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public BullishAbandonedBabyByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75M, decimal dojiThreshold = 0.1M) 
            : base(inputs, i => i, (i, otm) => otm, downTrendPeriodCount, longPeriodCount, longThreshold, dojiThreshold)
        {
        }
    }

    public class BullishAbandonedBaby : BullishAbandonedBaby<Candle, AnalyzableTick<bool?>>
    {
        public BullishAbandonedBaby(IEnumerable<Candle> inputs, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75M, decimal dojiThreshold = 0.1M) 
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), (i, otm) => new AnalyzableTick<bool?>(i.DateTime, otm), downTrendPeriodCount, longPeriodCount, longThreshold, dojiThreshold)
        {
        }
    }
}