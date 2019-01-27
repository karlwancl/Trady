using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_bearish_reversal_patterns#bearish_abandoned_baby
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class BullishAbandonedBaby<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private DownTrendByTuple _downTrend;
        private BearishLongDayByTuple _bearishLongDay;
        private BullishLongDayByTuple _bullishLongDay;
        private DojiByTuple _doji;

        public BullishAbandonedBaby(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75m, decimal dojiThreshold = 0.1m)
            : base(inputs, inputMapper)
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

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < 2)
                return default;

            if (!_doji[index - 1])
                return false;

            var isGapped = mappedInputs[index - 1].High < mappedInputs[index - 2].Low && mappedInputs[index - 1].High < mappedInputs[index].Low;
            return (_downTrend[index - 1] ?? false) && _bearishLongDay[index - 2] && isGapped && _bullishLongDay[index];
        }
    }

    public class BullishAbandonedBabyByTuple : BullishAbandonedBaby<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public BullishAbandonedBabyByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75M, decimal dojiThreshold = 0.1M)
            : base(inputs, i => i, downTrendPeriodCount, longPeriodCount, longThreshold, dojiThreshold)
        {
        }
    }

    public class BullishAbandonedBaby : BullishAbandonedBaby<IOhlcv, AnalyzableTick<bool?>>
    {
        public BullishAbandonedBaby(IEnumerable<IOhlcv> inputs, int downTrendPeriodCount = 3, int longPeriodCount = 20, decimal longThreshold = 0.75M, decimal dojiThreshold = 0.1M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), downTrendPeriodCount, longPeriodCount, longThreshold, dojiThreshold)
        {
        }
    }
}
