using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/m/morningstar.asp
    /// </summary>
    public class MorningStar<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private DownTrendByTuple _downTrend;
        private BullishLongDayByTuple _bullishLongDay;
        private ShortDayByTuple _shortDay;
        private BearishLongDayByTuple _bearishLongDay;

        public MorningStar(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int downTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m, decimal threshold = 0.1m) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);

            var ocs = mappedInputs.Select(c => (c.Open, c.Close));
            _downTrend = new DownTrendByTuple(mappedInputs.Select(c => (c.High, c.Low)), downTrendPeriodCount);
            _bullishLongDay = new BullishLongDayByTuple(ocs, periodCount, longThreshold);
            _shortDay = new ShortDayByTuple(ocs, periodCount, shortThreshold);
            _bearishLongDay = new BearishLongDayByTuple(ocs, periodCount, longThreshold);

            DownTrendPeriodCount = downTrendPeriodCount;
            PeriodCount = periodCount;
            LongThreshold = longThreshold;
            ShortThreshold = shortThreshold;
            Threshold = threshold;
        }

        public int DownTrendPeriodCount { get; }
        public int PeriodCount { get; }
        public decimal LongThreshold { get; }
        public decimal ShortThreshold { get; }
        public decimal Threshold { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < 2) return default;

            decimal midPoint(int i) => (mappedInputs[i].Open + mappedInputs[i].Close) / 2;

            return (_downTrend[index - 1] ?? false) &&
                _bearishLongDay[index - 2] &&
                _shortDay[index - 1] &&
                (mappedInputs[index - 1].Close < mappedInputs[index - 2].Close) &&
                _bullishLongDay[index] &&
                (mappedInputs[index].Open > Math.Max(mappedInputs[index - 1].Open, mappedInputs[index - 1].Close)) &&
                Math.Abs((mappedInputs[index].Close - midPoint(index - 2)) / midPoint(index - 2)) < Threshold;
        }
    }

    public class MorningStarByTuple : MorningStar<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public MorningStarByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M, decimal threshold = 0.1M)
            : base(inputs, i => i, downTrendPeriodCount, periodCount, shortThreshold, longThreshold, threshold)
        {
        }
    }

    public class MorningStar : MorningStar<IOhlcv, AnalyzableTick<bool?>>
    {
        public MorningStar(IEnumerable<IOhlcv> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M, decimal threshold = 0.1M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), downTrendPeriodCount, periodCount, shortThreshold, longThreshold, threshold)
        {
        }
    }
}
