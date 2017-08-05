using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class AroonOscillator<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low), decimal?, TOutput>
    {
        readonly AroonByTuple _aroon;

        public AroonOscillator(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low)> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount) : base(inputs, inputMapper, outputMapper)
        {
			_aroon = new AroonByTuple(inputs.Select(inputMapper), periodCount);
			PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low)> mappedInputs, int index)
        {
			var aroon = _aroon[index];
			return (aroon.Up - aroon.Down);
        }
    }

    public class AroonOscillatorByTuple : AroonOscillator<(decimal High, decimal Low), decimal?>
    {
        public AroonOscillatorByTuple(IEnumerable<(decimal High, decimal Low)> inputs, int periodCount) 
            : base(inputs, i => i, (i, otm) => otm, periodCount)
        {
        }
    }

    public class AroonOscillator : AroonOscillator<Candle, AnalyzableTick<decimal?>>
    {
        public AroonOscillator(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => (i.High, i.Low), (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), periodCount)
        {
        }
    }
}