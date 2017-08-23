using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/m/morningstar.asp
    /// </summary>
    public class MorningStar<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        DownTrendByTuple _downTrend;
        BullishLongDayByTuple _bullishLongDay;
        ShortDayByTuple _shortDay;
        BearishLongDayByTuple _bearishLongDay;

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

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < 2) return null;

            Func<int, decimal> midPoint = i => (mappedInputs.ElementAt(i).Open + mappedInputs.ElementAt(i).Close) / 2;

            return (_downTrend[index - 1] ?? false) &&
                _bearishLongDay[index - 2] &&
                _shortDay[index - 1] &&
                (mappedInputs.ElementAt(index - 1).Close < mappedInputs.ElementAt(index - 2).Close) &&
                _bullishLongDay[index] &&
                (mappedInputs.ElementAt(index).Open > Math.Max(mappedInputs.ElementAt(index - 1).Open, mappedInputs.ElementAt(index - 1).Close)) &&
                Math.Abs((mappedInputs.ElementAt(index).Close - midPoint(index - 2)) / midPoint(index - 2)) < Threshold;
        }
    }

    public class MorningStarByTuple : MorningStar<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public MorningStarByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M, decimal threshold = 0.1M) 
            : base(inputs, i => i, downTrendPeriodCount, periodCount, shortThreshold, longThreshold, threshold)
        {
        }
    }

    public class MorningStar : MorningStar<Candle, AnalyzableTick<bool?>>
    {
        public MorningStar(IEnumerable<Candle> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal shortThreshold = 0.25M, decimal longThreshold = 0.75M, decimal threshold = 0.1M) 
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), downTrendPeriodCount, periodCount, shortThreshold, longThreshold, threshold)
        {
        }
    }
}