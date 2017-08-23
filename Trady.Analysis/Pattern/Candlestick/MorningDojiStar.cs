using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class MorningDojiStar<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        DownTrendByTuple _downTrend;
        BullishLongDayByTuple _bullishLongDay;
        DojiByTuple _doji;
        BearishLongDayByTuple _bearishLongDay;

        public MorningDojiStar(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int downTrendPeriodCount = 3, int periodCount = 20, decimal longThreshold = 0.75m, decimal dojiThreshold = 0.25m, decimal threshold = 0.1m) 
            : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);

            var ocs = mappedInputs.Select(c => (c.Open, c.Close));
            _downTrend = new DownTrendByTuple(mappedInputs.Select(c => (c.High, c.Low)), downTrendPeriodCount);
            _bullishLongDay = new BullishLongDayByTuple(ocs, periodCount, longThreshold);
            _doji = new DojiByTuple(mappedInputs, dojiThreshold);
            _bearishLongDay = new BearishLongDayByTuple(ocs, periodCount, longThreshold);

            UpTrendPeriodCount = downTrendPeriodCount;
            PeriodCount = periodCount;
            LongThreshold = longThreshold;
            DojiThreshold = dojiThreshold;
            Threshold = threshold;
        }

        public int UpTrendPeriodCount { get; }
        public int PeriodCount { get; }
        public decimal LongThreshold { get; }
        public decimal DojiThreshold { get; }
        public decimal Threshold { get; }

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < 2) return null;

            Func<int, decimal> midPoint = i => (mappedInputs.ElementAt(i).Open + mappedInputs.ElementAt(i).Close) / 2;

            return (_downTrend[index - 1] ?? false) &&
                _bearishLongDay[index - 2] &&
                _doji[index - 1] &&
                (midPoint(index - 1) < mappedInputs.ElementAt(index - 2).Close) &&
                _bullishLongDay[index] &&
                (mappedInputs.ElementAt(index).Open > Math.Max(mappedInputs.ElementAt(index - 1).Open, mappedInputs.ElementAt(index - 1).Close)) && 
                Math.Abs((mappedInputs.ElementAt(index).Close - midPoint(index - 2)) / midPoint(index - 2)) < Threshold;
        }
    }

    public class MoringinDojiStarByTuple : MorningDojiStar<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public MoringinDojiStarByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal longThreshold = 0.75M, decimal dojiThreshold = 0.25M, decimal threshold = 0.1M)
            : base(inputs, i => i, downTrendPeriodCount, periodCount, longThreshold, dojiThreshold, threshold)
        {
        }
    }

    public class MorningDojiStar : MorningDojiStar<Candle, AnalyzableTick<bool?>>
    {
        public MorningDojiStar(IEnumerable<Candle> inputs, int downTrendPeriodCount = 3, int periodCount = 20, decimal longThreshold = 0.75M, decimal dojiThreshold = 0.25M, decimal threshold = 0.1M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), downTrendPeriodCount, periodCount, longThreshold, dojiThreshold, threshold)
        {
        }
    }
}