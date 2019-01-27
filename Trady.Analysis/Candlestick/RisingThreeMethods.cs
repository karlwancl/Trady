using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/r/rising-three-methods.asp
    /// </summary>
    public class RisingThreeMethods<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private readonly UpTrendByTuple _upTrend;
        private readonly ShortDayByTuple _shortDay;
        private readonly BullishLongDayByTuple _bullishLongDay;

        public RisingThreeMethods(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            var ocs = mappedInputs.Select(i => (i.Open, i.Close));

            _upTrend = new UpTrendByTuple(mappedInputs.Select(i => (i.High, i.Low)), upTrendPeriodCount);
            _shortDay = new ShortDayByTuple(ocs, periodCount, shortThreshold);
            _bullishLongDay = new BullishLongDayByTuple(ocs, periodCount, longThreshold);

            UpTrendPeriodCount = upTrendPeriodCount;
            PeriodCount = periodCount;
            ShortThreshold = shortThreshold;
            LongThreshold = longThreshold;
        }

        public int UpTrendPeriodCount { get; }
        public int PeriodCount { get; }
        public decimal ShortThreshold { get; }
        public decimal LongThreshold { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index == 0)
                return default;

            if (!_bullishLongDay[index] || !_shortDay[index - 1])
                return false;

            bool isDesc(int i) => mappedInputs[i].Close < mappedInputs[i - 1].Close && mappedInputs[i].Open < mappedInputs[i - 1].Open;
            for (int i = index - 1; i >= UpTrendPeriodCount; i--)
            {
                if (_shortDay[i] && !_bullishLongDay[i - 1] && !isDesc(i))
                    return false;
                else if (_bullishLongDay[i])
                    return (mappedInputs[index].High > mappedInputs[i].High) &&
                        mappedInputs[i].High > mappedInputs[i + 1].High &&
                        mappedInputs[i].Low < mappedInputs[index - 1].Low &&
                        (_upTrend[i] ?? false);
            }
            return false;
        }
    }

    public class RisingThreeMethodsByTuple : RisingThreeMethods<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public RisingThreeMethodsByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M)
            : base(inputs, i => i, upTrendPeriodCount, periodCount, shortThreshold, longThreshold)
        {
        }
    }

    public class RisingThreeMethods : RisingThreeMethods<IOhlcv, AnalyzableTick<bool?>>
    {
        public RisingThreeMethods(IEnumerable<IOhlcv> inputs, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), upTrendPeriodCount, periodCount, shortThreshold, longThreshold)
        {
        }
    }
}
