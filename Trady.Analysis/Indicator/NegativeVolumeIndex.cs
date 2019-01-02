using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class NegativeVolumeIndex<TInput, TOutput> : CumulativeNumericAnalyzableBase<TInput, (decimal Close, decimal Volume), TOutput>
    {
        public NegativeVolumeIndex(IEnumerable<TInput> inputs, Func<TInput, (decimal Close, decimal Volume)> inputMapper)
            : base(inputs, inputMapper)
        {
        }

        protected override decimal? ComputeCumulativeValue(IReadOnlyList<(decimal Close, decimal Volume)> mappedInputs, int index, decimal? prevOutputToMap)
        {
            if (index - 1 < 0)
                return default;

            if (mappedInputs[index].Volume >= mappedInputs[index - 1].Volume)
                return prevOutputToMap;

            return prevOutputToMap * (1 + (mappedInputs[index].Close - mappedInputs[index - 1].Close) / mappedInputs[index - 1].Close);
        }

        protected override decimal? ComputeInitialValue(IReadOnlyList<(decimal Close, decimal Volume)> mappedInputs, int index) => 100;
    }

    public class NegativeVolumeIndexByTuple : NegativeVolumeIndex<(decimal Close, decimal Volume), decimal?>
    {
        public NegativeVolumeIndexByTuple(IEnumerable<(decimal Close, decimal Volume)> inputs)
            : base(inputs, i => i)
        {
        }
    }

    public class NegativeVolumeIndex : NegativeVolumeIndex<IOhlcv, AnalyzableTick<decimal?>>
    {
        public NegativeVolumeIndex(IEnumerable<IOhlcv> inputs)
            : base(inputs, i => (i.Close, i.Volume))
        {
        }
    }
}
