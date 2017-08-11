using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ClosePriceChangeTrend<TInput, TOutput> : AnalyzableBase<TInput, decimal, Trend?, TOutput>
    {
        readonly ClosePriceChangeByTuple _closePriceChange;

        public ClosePriceChangeTrend(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, Trend?, TOutput> outputMapper) : base(inputs, inputMapper, outputMapper)
        {
			_closePriceChange = new ClosePriceChangeByTuple(inputs.Select(inputMapper));
		}

        protected override Trend? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
			=> StateHelper.IsTrending(_closePriceChange[index]);
	}

    public class ClosePriceChangeTrendByTuple : ClosePriceChangeTrend<decimal, Trend?>
    {
        public ClosePriceChangeTrendByTuple(IEnumerable<decimal> inputs) 
            : base(inputs, i => i, (i, otm) => otm)
        {
        }
    }

    public class ClosePriceChangeTrend : ClosePriceChangeTrend<Candle, AnalyzableTick<Trend?>>
    {
        public ClosePriceChangeTrend(IEnumerable<Candle> inputs)
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<Trend?>(i.DateTime, otm))
        {
        }
    }
}