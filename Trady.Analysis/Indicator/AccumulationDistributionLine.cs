using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Extension;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class AccumulationDistributionLine<TInput, TOutput>
        : CumulativeNumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close, decimal Volume), TOutput>
    {
        protected AccumulationDistributionLine(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close, decimal Volume)> inputMapper) : base(inputs, inputMapper)
        {
        }

        protected override int InitialValueIndex => 0;

        protected override decimal? ComputeNullValue(IReadOnlyList<(decimal High, decimal Low, decimal Close, decimal Volume)> mappedInputs, int index) => null;

        protected override decimal? ComputeInitialValue(IReadOnlyList<(decimal High, decimal Low, decimal Close, decimal Volume)> mappedInputs, int index) => mappedInputs.ElementAt(index).Volume;

        protected override decimal? ComputeCumulativeValue(IReadOnlyList<(decimal High, decimal Low, decimal Close, decimal Volume)> mappedInputs, int index, decimal? prevOutputToMap)
        {
            var (High, Low, Close, Volume) = mappedInputs[index];
            var prevInput = mappedInputs[index - 1];

            decimal ratio;
            if (High == Low)
            {
                if (prevInput.Close == 0)
                    return default;
                ratio = (Close / prevInput.Close) - 1;
            }
            else
                ratio = (Close * 2 - Low - High) / (High - Low);

            return prevOutputToMap + ratio * Volume;
        }
    }

    public class AccumulationDistributionLineByTuple : AccumulationDistributionLine<(decimal High, decimal Low, decimal Close, decimal Volume), decimal?>
    {
        public AccumulationDistributionLineByTuple(IEnumerable<(decimal High, decimal Low, decimal Close, decimal Volume)> inputs)
            : base(inputs, i => i)
        {
        }
    }

    public class AccumulationDistributionLine : AccumulationDistributionLine<IOhlcv, AnalyzableTick<decimal?>>
    {
        public AccumulationDistributionLine(IEnumerable<IOhlcv> inputs)
            : base(inputs, c => (c.High, c.Low, c.Close, c.Volume))
        {
        }
    }
}
