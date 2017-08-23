using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class MovingAverageConvergenceDivergenceOscillatorTrend<TInput, TOutput> : AnalyzableBase<TInput, decimal, Trend?, TOutput>
    {
        readonly MovingAverageConvergenceDivergenceByTuple _macd;

        public MovingAverageConvergenceDivergenceOscillatorTrend(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) : base(inputs, inputMapper)
        {
			_macd = new MovingAverageConvergenceDivergenceByTuple(inputs.Select(inputMapper), emaPeriodCount1, emaPeriodCount2, demPeriodCount);
		}

        protected override Trend? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
			=> index >= 1 ? StateHelper.IsTrending(_macd[index].MacdHistogram, _macd[index - 1].MacdHistogram) : null;
	}

    public class MovingAverageConvergenceDivergenceOscillatorTrendByTuple : MovingAverageConvergenceDivergenceOscillatorTrend<decimal, Trend?>
    {
        public MovingAverageConvergenceDivergenceOscillatorTrendByTuple(IEnumerable<decimal> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) 
            : base(inputs, i => i, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }
    }

    public class MovingAverageConvergenceDivergenceOscillatorTrend : MovingAverageConvergenceDivergenceOscillatorTrend<Candle, AnalyzableTick<Trend?>>
    {
        public MovingAverageConvergenceDivergenceOscillatorTrend(IEnumerable<Candle> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(inputs, i => i.Close, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }
    }
}