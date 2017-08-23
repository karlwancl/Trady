using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class UpTrend<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low), bool?, TOutput>
    {
        public UpTrend(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low)> inputMapper, int periodCount = 3) : base(inputs, inputMapper)
        {
			PeriodCount = periodCount;
		}

        public int PeriodCount { get; }

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low)> mappedInputs, int index)
        {
			if (index < PeriodCount - 1)
				return null;

			for (int i = 0; i < PeriodCount; i++)
			{
                bool isHighIncreasing = mappedInputs.ElementAt(index - i).High > mappedInputs.ElementAt(index - i - 1).High;
                bool isLowIncreasing = mappedInputs.ElementAt(index - i).Low > mappedInputs.ElementAt(index - i - 1).Low;
				if (!isHighIncreasing || !isLowIncreasing)
					return false;
			}

			return true;        
        }
    }

    public class UpTrendByTuple : UpTrend<(decimal High, decimal Low), bool?>
    {
        public UpTrendByTuple(IEnumerable<(decimal High, decimal Low)> inputs, int periodCount = 3) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class UpTrend : UpTrend<Candle, AnalyzableTick<bool?>>
    {
        public UpTrend(IEnumerable<Candle> inputs, int periodCount = 3) 
            : base(inputs, i => (i.High, i.Low), periodCount)
        {
        }
    }
}
