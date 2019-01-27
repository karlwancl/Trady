using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/f/falling-three-methods.asp
    /// </summary>
    public class FallingThreeMethods<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private DownTrendByTuple _downTrend;
        private ShortDayByTuple _shortDay;
        private BearishLongDayByTuple _bearishLongDay;

        public FallingThreeMethods(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int downTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);

            var ocs = mappedInputs.Select(i => (i.Open, i.Close));

            _downTrend = new DownTrendByTuple(mappedInputs.Select(i => (i.High, i.Low)), downTrendPeriodCount);
            _shortDay = new ShortDayByTuple(ocs, periodCount, shortThreshold);
            _bearishLongDay = new BearishLongDayByTuple(ocs, periodCount, longThreshold);

            DownTrendPeriodCount = downTrendPeriodCount;
            PeriodCount = periodCount;
            ShortThreshold = shortThreshold;
            LongThreshold = longThreshold;
        }

        public int DownTrendPeriodCount { get; }
        public int PeriodCount { get; }
        public decimal ShortThreshold { get; }
        public decimal LongThreshold { get; }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index == 0)
                return default;

            if (!_bearishLongDay[index] || !_shortDay[index - 1])
                return false;

            bool isAsc(int i) => mappedInputs[i].Close > mappedInputs[i - 1].Close && mappedInputs[i].Open > mappedInputs[i - 1].Open;
            for (var i = index - 1; i >= DownTrendPeriodCount; i--)
            {
                if (_shortDay[i] && !_bearishLongDay[i - 1] && !isAsc(i))
                    return false;
                else if (_bearishLongDay[i])
                    return (mappedInputs[index].Low < mappedInputs[i].Low) &&
                        (mappedInputs[i].Low < mappedInputs[i + 1].Low) &&
                        (mappedInputs[i].High > mappedInputs[index - 1].High) &&
                        (_downTrend[i] ?? false);
            }
            return false;
        }
    }

    public class FallingThreeMethodsByTuple : FallingThreeMethods<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public FallingThreeMethodsByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M)
            : base(inputs, i => i, downTrendPeriodCount, periodCount, shortThreshold, longThreshold)
        {
        }
    }

    public class FallingThreeMethods : FallingThreeMethods<IOhlcv, AnalyzableTick<bool?>>
    {
        public FallingThreeMethods(IEnumerable<IOhlcv> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), downTrendPeriodCount, periodCount, shortThreshold, longThreshold)
        {
        }
    }
}
