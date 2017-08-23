using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsLowestPrice<TInput, TOutput> : AnalyzableBase<TInput, decimal, bool?, TOutput>
    {
        readonly int _periodCount;

        public IsLowestPrice(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
			_periodCount = periodCount;
		}

        protected override bool? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
            => mappedInputs.Skip(mappedInputs.Count() - _periodCount).Min() == mappedInputs.ElementAt(index);
	}

    public class IsLowestPriceByTuple : IsLowestPrice<decimal, bool?>
    {
        public IsLowestPriceByTuple(IEnumerable<decimal> inputs, int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class IsLowestPrice : IsLowestPrice<Candle, AnalyzableTick<bool?>>
    {
        public IsLowestPrice(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}