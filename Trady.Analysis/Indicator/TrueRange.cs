using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class TrueRange<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), decimal?, TOutput>
    {
        public TrueRange(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override decimal? ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
            => index > 0 ? new List<decimal?> {
	            mappedInputs.ElementAt(index).High - mappedInputs.ElementAt(index).Low,
	            Math.Abs(mappedInputs.ElementAt(index).High - mappedInputs.ElementAt(index - 1).Close),
	            Math.Abs(mappedInputs.ElementAt(index).Low - mappedInputs.ElementAt(index - 1).Close) }.Max() : null;

    }

    public class TrueRangeByTuple : TrueRange<(decimal High, decimal Low, decimal Close), decimal?>
    {
        public TrueRangeByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs) 
            : base(inputs, i => i)
        {
        }
    }

    public class TrueRange : TrueRange<Candle, AnalyzableTick<decimal?>>
    {
        public TrueRange(IEnumerable<Candle> inputs) 
            : base(inputs, i => (i.High, i.Low, i.Close))
        {
        }
    }
}
