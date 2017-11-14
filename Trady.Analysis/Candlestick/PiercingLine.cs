using System;
using System.Collections.Generic;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class PiercingLine<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        public PiercingLine(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            throw new NotImplementedException();
        }
    }

    public class PiercingLineByTuple : PiercingLine<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public PiercingLineByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs)
            : base(inputs, i => i)
        {
        }
    }

    public class PiercingLine : PiercingLine<IOhlcv, AnalyzableTick<bool?>>
    {
        public PiercingLine(IEnumerable<IOhlcv> inputs)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close))
        {
        }
    }
}