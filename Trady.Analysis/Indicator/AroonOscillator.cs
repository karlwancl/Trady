using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class AroonOscillator<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low), TOutput>
    {
        private readonly AroonByTuple _aroon;

        public AroonOscillator(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low)> inputMapper, int periodCount)
            : base(inputs, inputMapper)
        {
            _aroon = new AroonByTuple(inputs.Select(inputMapper), periodCount);
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low)> mappedInputs, int index)
        {
            var (Up, Down) = _aroon[index];
            return Up - Down;
        }
    }

    public class AroonOscillatorByTuple : AroonOscillator<(decimal High, decimal Low), decimal?>
    {
        public AroonOscillatorByTuple(IEnumerable<(decimal High, decimal Low)> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class AroonOscillator : AroonOscillator<IOhlcv, AnalyzableTick<decimal?>>
    {
        public AroonOscillator(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => (i.High, i.Low), periodCount)
        {
        }
    }
}
