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
    public class ShortDay : AnalyzableBase<(decimal Open, decimal Close), bool?>
    {
        public ShortDay(IList<Candle> candles, int periodCount, decimal threshold)
            : this(candles.Select(c => (c.Open, c.Close)).ToList(), periodCount, threshold)
        {
        }

        public ShortDay(IList<(decimal Open, decimal Close)> inputs, int periodCount, decimal threshold) 
            : base(inputs)
        {
            PeriodCount = periodCount;
            Threshold = threshold;
        }

        public int PeriodCount { get; private set; }

        public decimal Threshold { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            var bodyLengths = Inputs.Select(i => Math.Abs(i.Close - i.Open)).ToList();
            return bodyLengths[index] < bodyLengths.Percentile(PeriodCount, index, Threshold);
        }
    }
}