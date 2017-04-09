using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class Doji : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public Doji(IList<Candle> candles, decimal threshold)
            : this(candles.Select(c => (c.Open, c.High, c.Low, c.Close)).ToList(), threshold)
        {
        }

        public Doji(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, decimal threshold) : base(inputs)
        {
            Threshold = threshold;
        }

        public decimal Threshold { get; set; }

        protected override bool? ComputeByIndexImpl(int index)
            => Math.Abs(Inputs[index].Close - Inputs[index].Open) < Threshold * Math.Abs(Inputs[index].High - Inputs[index].Low); 
    }
}