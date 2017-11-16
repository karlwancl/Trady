using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class OnBalanceVolume<TInput, TOutput> : CumulativeNumericAnalyzableBase<TInput, (decimal Close, decimal Volume), TOutput>
    {
        public OnBalanceVolume(IEnumerable<TInput> inputs, Func<TInput, (decimal Close, decimal Volume)> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override decimal? ComputeInitialValue(IReadOnlyList<(decimal Close, decimal Volume)> mappedInputs, int index) => mappedInputs.ElementAt(index).Volume;

        protected override decimal? ComputeCumulativeValue(IReadOnlyList<(decimal Close, decimal Volume)> mappedInputs, int index, decimal? prevOutputToMap)
        {
            var input = mappedInputs[index];
            var prevInput = mappedInputs[index - 1];
            decimal increment = input.Volume * (input.Close > prevInput.Close ? 1 : (input.Close == prevInput.Close ? 0 : -1));
            return prevOutputToMap + increment;
        }
    }

    public class OnBalanceVolumeByTuple : OnBalanceVolume<(decimal Close, decimal Volume), decimal?>
    {
        public OnBalanceVolumeByTuple(IEnumerable<(decimal Close, decimal Volume)> inputs)
            : base(inputs, i => i)
        {
        }
    }

    public class OnBalanceVolume : OnBalanceVolume<IOhlcv, AnalyzableTick<decimal?>>
    {
        public OnBalanceVolume(IEnumerable<IOhlcv> inputs)
            : base(inputs, i => (i.Close, i.Volume))
        {
        }
    }
}