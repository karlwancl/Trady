using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class SimpleMovingAverageOscillator<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        private readonly SimpleMovingAverageByTuple _sma2;
        private readonly SimpleMovingAverageByTuple _sma1;

        public SimpleMovingAverageOscillator(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount1, int periodCount2) : base(inputs, inputMapper)
        {
            _sma1 = new SimpleMovingAverageByTuple(inputs.Select(inputMapper), periodCount1);
            _sma2 = new SimpleMovingAverageByTuple(inputs.Select(inputMapper), periodCount2);

            PeriodCount1 = periodCount1;
            PeriodCount2 = periodCount2;
        }

        public int PeriodCount1 { get; }

        public int PeriodCount2 { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index) => _sma1[index] - _sma2[index];
    }

    public class SimpleMovingAverageOscillatorByTuple : SimpleMovingAverageOscillator<decimal, decimal?>
    {
        public SimpleMovingAverageOscillatorByTuple(IEnumerable<decimal> inputs, int periodCount1, int periodCount2)
            : base(inputs, i => i, periodCount1, periodCount2)
        {
        }
    }

    public class SimpleMovingAverageOscillator : SimpleMovingAverageOscillator<IOhlcv, AnalyzableTick<decimal?>>
    {
        public SimpleMovingAverageOscillator(IEnumerable<IOhlcv> inputs, int periodCount1, int periodCount2)
            : base(inputs, i => i.Close, periodCount1, periodCount2)
        {
        }
    }
}