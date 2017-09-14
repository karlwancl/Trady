using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageTrend<TInput, TOutput> : AnalyzableBase<TInput, decimal, Trend?, TOutput>
    {
        private readonly ExponentialMovingAverageByTuple _ema;

        public ExponentialMovingAverageTrend(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _ema = new ExponentialMovingAverageByTuple(inputs.Select(inputMapper), periodCount);
        }

        protected override Trend? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => StateHelper.IsTrending(_ema[index]);
    }

    public class ExponentialMovingAverageTrendByTuple : ExponentialMovingAverageTrend<decimal, Trend?>
    {
        public ExponentialMovingAverageTrendByTuple(IEnumerable<decimal> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class ExponentialMovingAverageTrend : ExponentialMovingAverageTrend<Candle, AnalyzableTick<Trend?>>
    {
        public ExponentialMovingAverageTrend(IEnumerable<Candle> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}