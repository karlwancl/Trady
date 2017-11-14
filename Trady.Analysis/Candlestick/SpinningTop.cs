using System;
using System.Collections.Generic;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class SpinningTop<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        public SpinningTop(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            throw new NotImplementedException();
        }
    }

    public class SpinningTopByTuple : SpinningTop<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public SpinningTopByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs)
            : base(inputs, i => i)
        {
        }
    }

    public class SpinningTop : SpinningTop<IOhlcv, AnalyzableTick<bool?>>
    {
        public SpinningTop(IEnumerable<IOhlcv> inputs)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close))
        {
        }
    }
}