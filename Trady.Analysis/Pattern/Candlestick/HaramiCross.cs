using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class HaramiCross<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        public HaramiCross(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, Func<TInput, bool?, TOutput> outputMapper) : base(inputs, inputMapper, outputMapper)
        {
        }

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            throw new NotImplementedException();
        }
    }

    public class HaramiCrossByTuple : HaramiCross<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public HaramiCrossByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs) 
            : base(inputs, i => i, (i, otm) => otm)
        {
        }
    }

    public class HaramiCross : HaramiCross<Candle, AnalyzableTick<bool?>>
    {
        public HaramiCross(IEnumerable<Candle> inputs)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), (i, otm) => new AnalyzableTick<bool?>(i.DateTime, otm))
        {
        }
    }
}