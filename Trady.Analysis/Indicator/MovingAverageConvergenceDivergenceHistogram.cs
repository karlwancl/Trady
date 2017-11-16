using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class MovingAverageConvergenceDivergenceHistogram<TInput, TOutput> 
        : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        private MovingAverageConvergenceDivergenceByTuple _macd;

        public MovingAverageConvergenceDivergenceHistogram(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) : base(inputs, inputMapper)
        {
            _macd = new MovingAverageConvergenceDivergenceByTuple(inputs.Select(inputMapper), emaPeriodCount1, emaPeriodCount2, demPeriodCount);

            EmaPeriodCount1 = emaPeriodCount1;
            EmaPeriodCount2 = emaPeriodCount2;
            DemPeriodCount = demPeriodCount;
        }

        public int EmaPeriodCount1 { get; }

        public int EmaPeriodCount2 { get; }

        public int DemPeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index) => _macd[index].MacdHistogram;
    }

    public class MovingAverageConvergenceDivergenceHistogramByTuple : MovingAverageConvergenceDivergenceHistogram<decimal, decimal?>
    {
        public MovingAverageConvergenceDivergenceHistogramByTuple(IEnumerable<decimal> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(inputs, i => i, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }
    }

    public class MovingAverageConvergenceDivergenceHistogram : MovingAverageConvergenceDivergenceHistogram<IOhlcv, AnalyzableTick<decimal?>>
    {
        public MovingAverageConvergenceDivergenceHistogram(IEnumerable<IOhlcv> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(inputs, i => i.Close, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }
    }
}