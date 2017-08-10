using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/r/rising-three-methods.asp
    /// </summary>
    public class RisingThreeMethods<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        readonly UpTrendByTuple _upTrend;
        readonly ShortDayByTuple _shortDay;
        readonly BullishLongDayByTuple _bullishLongDay;

        public RisingThreeMethods(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, Func<TInput, bool?, TOutput> outputMapper, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25m, decimal longThreshold = 0.75m) : base(inputs, inputMapper, outputMapper)
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

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index == 0)
                return null;

            if (!_bullishLongDay[index] || !_shortDay[index - 1])
                return false;

            Func<int, bool> isDesc = i => mappedInputs.ElementAt(i).Close < mappedInputs.ElementAt(i - 1).Close && mappedInputs.ElementAt(i).Open < mappedInputs.ElementAt(i - 1).Open;
            for (int i = index - 1; i >= UpTrendPeriodCount; i--)
            {
                if (_shortDay[i] && !_bullishLongDay[i - 1] && !isDesc(i))
                    return false;
                else if (_bullishLongDay[i])
                    return (mappedInputs.ElementAt(index).High > mappedInputs.ElementAt(i).High) &&
                        mappedInputs.ElementAt(i).High > mappedInputs.ElementAt(i + 1).High &&
                        mappedInputs.ElementAt(i).Low < mappedInputs.ElementAt(index - 1).Low &&
                        (_upTrend[i] ?? false);
            }
            return false;
        }
    }

    public class RisingThreeMethodsByTuple : RisingThreeMethods<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public RisingThreeMethodsByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M) 
            : base(inputs, i => i, (i, otm) => otm, upTrendPeriodCount, periodCount, shortThreshold, longThreshold)
        {
        }
    }

    public class RisingThreeMethods : RisingThreeMethods<Candle, AnalyzableTick<bool?>>
    {
        public RisingThreeMethods(IEnumerable<Candle> inputs, int upTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M) 
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), (i, otm) => new AnalyzableTick<bool?>(i.DateTime, otm), upTrendPeriodCount, periodCount, shortThreshold, longThreshold)
        {
        }
    }
}