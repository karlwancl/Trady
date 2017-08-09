using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class DownTrend<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low), bool?, TOutput>
    {
        public DownTrend(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low)> inputMapper, Func<TInput, bool?, TOutput> outputMapper, int periodCount = 3) : base(inputs, inputMapper, outputMapper)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low)> mappedInputs, int index)
        {
			if (index < PeriodCount - 1)
				return null;

			for (int i = 0; i < PeriodCount; i++)
			{
                bool isHighDecreasing = mappedInputs.ElementAt(index - i).High < mappedInputs.ElementAt(index - i - 1).High;
                bool isLowDecreasing = mappedInputs.ElementAt(index - i).Low < mappedInputs.ElementAt(index - i - 1).Low;
				if (!isHighDecreasing || !isLowDecreasing)
					return false;
			}

			return true;
        }
    }

    public class DownTrendByTuple : DownTrend<(decimal High, decimal Low), bool?>
    {
        public DownTrendByTuple(IEnumerable<(decimal High, decimal Low)> inputs, int periodCount = 3) 
            : base(inputs, i => i, (i, otm) => otm, periodCount)
        {
        }
    }

    public class DownTrend : DownTrend<Candle, AnalyzableTick<bool?>>
    {
        public DownTrend(IEnumerable<Candle> inputs, int periodCount = 3) 
            : base(inputs, i => (i.High, i.Low), (i, otm) => new AnalyzableTick<bool?>(i.DateTime, otm), periodCount)
        {
        }
    }
}
