using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class MovingAverageConvergenceDivergenceCrossover<TInput, TOutput> : AnalyzableBase<TInput, decimal, Crossover?, TOutput>
    {
        private readonly MovingAverageConvergenceDivergenceByTuple _macd;

        public MovingAverageConvergenceDivergenceCrossover(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) : base(inputs, inputMapper)
        {
            _macd = new MovingAverageConvergenceDivergenceByTuple(inputs.Select(inputMapper), emaPeriodCount1, emaPeriodCount2, demPeriodCount);
        }

        protected override Crossover? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => index >= 1 ? StateHelper.IsCrossover(_macd[index].MacdHistogram, _macd[index - 1].MacdHistogram) : null;
    }

    public class MovingAverageConvergenceDivergenceCrossoverByTuple : MovingAverageConvergenceDivergenceCrossover<decimal, Crossover?>
    {
        public MovingAverageConvergenceDivergenceCrossoverByTuple(IEnumerable<decimal> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(inputs, i => i, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }
    }

    public class MovingAverageConvergenceDivergenceCrossover : MovingAverageConvergenceDivergenceCrossover<Candle, AnalyzableTick<Crossover?>>
    {
        public MovingAverageConvergenceDivergenceCrossover(IEnumerable<Candle> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(inputs, i => i.Close, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }
    }
}