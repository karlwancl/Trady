using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/m/eveningstar.asp
    /// </summary>
    public class EveningStar<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private UpTrendByTuple _upTrend;
        private BullishLongDayByTuple _bullishLongDay;
        private ShortDayByTuple _shortDay;
        private BearishLongDayByTuple _bearishLongDay;

        public EveningStar(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m, decimal threshold = 0.1m)
            : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);

            var ocs = mappedInputs.Select(c => (c.Open, c.Close));
            _upTrend = new UpTrendByTuple(mappedInputs.Select(c => (c.High, c.Low)), upTrendPeriodCount);
            _bullishLongDay = new BullishLongDayByTuple(ocs, periodCount, longThreshold);
            _shortDay = new ShortDayByTuple(ocs, periodCount, shortThreshold);
            _bearishLongDay = new BearishLongDayByTuple(ocs, periodCount, longThreshold);

            UpTrendPeriodCount = upTrendPeriodCount;
            PeriodCount = periodCount;
            LongThreshold = longThreshold;
            ShortThreshold = shortThreshold;
            Threshold = threshold;
        }

        public int UpTrendPeriodCount { get; }
        public int PeriodCount { get; }
        public decimal LongThreshold { get; }
        public decimal ShortThreshold { get; }
        public decimal Threshold { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < 2) return default;

            decimal midPoint(int i) => (mappedInputs[i].Open + mappedInputs[i].Close) / 2;

            return (_upTrend[index - 1] ?? false) &&
                _bullishLongDay[index - 2] &&
                _shortDay[index - 1] &&
                (mappedInputs[index - 1].Close > mappedInputs[index - 2].Close) &&
                _bearishLongDay[index] &&
                (mappedInputs[index].Open < Math.Min(mappedInputs[index - 1].Open, mappedInputs[index - 1].Close)) &&
                Math.Abs((mappedInputs[index].Close - midPoint(index - 2)) / midPoint(index - 2)) < Threshold;
        }
    }

    public class EveningStarByTuple : EveningStar<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public EveningStarByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M, decimal threshold = 0.1M)
            : base(inputs, i => i, upTrendPeriodCount, periodCount, shortThreshold, longThreshold, threshold)
        {
        }
    }

    public class EveningStar : EveningStar<IOhlcv, AnalyzableTick<bool?>>
    {
        public EveningStar(IEnumerable<IOhlcv> inputs, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M, decimal threshold = 0.1M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), upTrendPeriodCount, periodCount, shortThreshold, longThreshold, threshold)
        {
        }
    }
}
