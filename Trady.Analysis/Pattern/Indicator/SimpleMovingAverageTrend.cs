using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageTrend<TInput, TOutput> : AnalyzableBase<TInput, decimal, Trend?, TOutput>
    {
        readonly SimpleMovingAverageByTuple _sma;

        public SimpleMovingAverageTrend(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
			_sma = new SimpleMovingAverageByTuple(inputs.Select(inputMapper), periodCount);
		}

        protected override Trend? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
			=> index >= 1 ? StateHelper.IsTrending(_sma[index], _sma[index - 1]) : null;
	}

    public class SimpleMovingAverageTrendByTuple : SimpleMovingAverageTrend<decimal, Trend?>
    {
        public SimpleMovingAverageTrendByTuple(IEnumerable<decimal> inputs, int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class SimpleMovingAverageTrend : SimpleMovingAverageTrend<Candle, AnalyzableTick<Trend?>>
    {
        public SimpleMovingAverageTrend(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}