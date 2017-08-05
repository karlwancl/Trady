using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class SimpleMovingAverageOscillator<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        readonly SimpleMovingAverageByTuple _sma2;
        readonly SimpleMovingAverageByTuple _sma1;

        public SimpleMovingAverageOscillator(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount1, int periodCount2) : base(inputs, inputMapper, outputMapper)
        {
			_sma1 = new SimpleMovingAverageByTuple(inputs.Select(inputMapper), periodCount1);
			_sma2 = new SimpleMovingAverageByTuple(inputs.Select(inputMapper), periodCount2);

			PeriodCount1 = periodCount1;
			PeriodCount2 = periodCount2;
        }

        public int PeriodCount1 { get; private set; }

        public int PeriodCount2 { get; private set; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index) => _sma1[index] - _sma2[index];
    }

    public class SimpleMovingAverageOscillatorByTuple : SimpleMovingAverageOscillator<decimal, decimal?>
    {
        public SimpleMovingAverageOscillatorByTuple(IEnumerable<decimal> inputs, int periodCount1, int periodCount2) 
            : base(inputs, i => i, (i, otm) => otm, periodCount1, periodCount2)
        {
        }
    }

    public class SimpleMovingAverageOscillator : SimpleMovingAverageOscillator<Candle, AnalyzableTick<decimal?>>
    {
        public SimpleMovingAverageOscillator(IEnumerable<Candle> inputs, int periodCount1, int periodCount2) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), periodCount1, periodCount2)
        {
        }
    }
}