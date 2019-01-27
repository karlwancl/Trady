using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class PositiveVolumeIndex<TInput, TOutput> : CumulativeNumericAnalyzableBase<TInput, (decimal Close, decimal Volume), TOutput>
    {
        public PositiveVolumeIndex(IEnumerable<TInput> inputs, Func<TInput, (decimal Close, decimal Volume)> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override decimal? ComputeCumulativeValue(IReadOnlyList<(decimal Close, decimal Volume)> mappedInputs, int index, decimal? prevOutputToMap)
        {
            if (index - 1 < 0)
                return default;

            if (mappedInputs[index].Volume <= mappedInputs[index - 1].Volume)
                return prevOutputToMap;

            return prevOutputToMap * (1 + (mappedInputs[index].Close - mappedInputs[index - 1].Close) / mappedInputs[index - 1].Close);
        }

        protected override decimal? ComputeInitialValue(IReadOnlyList<(decimal Close, decimal Volume)> mappedInputs, int index) => 100;
    }

    public class PositiveVolumeIndexByTuple : PositiveVolumeIndex<(decimal Close, decimal Volume), decimal?>
    {
        public PositiveVolumeIndexByTuple(IEnumerable<(decimal Close, decimal Volume)> inputs) : base(inputs, i => i)
        {
        }
    }

    public class PositiveVolumeIndex : PositiveVolumeIndex<IOhlcv, AnalyzableTick<decimal?>>
    {
        public PositiveVolumeIndex(IEnumerable<IOhlcv> inputs) : base(inputs, i => (i.Close, i.Volume))
        {
        }
    }
}
