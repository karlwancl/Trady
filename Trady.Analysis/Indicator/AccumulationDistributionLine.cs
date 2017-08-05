using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class AccumulationDistributionLine<TInput, TOutput> 
        : CumulativeAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close, decimal Volume), decimal?, TOutput>
    {
        public AccumulationDistributionLine(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close, decimal Volume)> inputMapper, Func<TInput, decimal?, TOutput> outputMapper) : base(inputs, inputMapper, outputMapper)
        {
        }

        protected override int InitialValueIndex => 0;

        protected override decimal? ComputeNullValue(IEnumerable<(decimal High, decimal Low, decimal Close, decimal Volume)> mappedInputs, int index) => null;

        protected override decimal? ComputeInitialValue(IEnumerable<(decimal High, decimal Low, decimal Close, decimal Volume)> mappedInputs, int index) => mappedInputs(index).Volume;

        protected override decimal? ComputeCumulativeValue(IEnumerable<(decimal High, decimal Low, decimal Close, decimal Volume)> mappedInputs, int index, decimal? prevOutputToMap)
        {
            var input = mappedInputs.ElementAt(index);
            var prevInput = mappedInputs.ElementAt(index - 1);

			decimal ratio = (input.High == input.Low) ?
				(input.Close / prevInput.Close) - 1 :
				(input.Close * 2 - input.Low - input.High) / (input.High - input.Low);

			return prevOutputToMap + ratio * input.Volume;
        }
    }

    public class AccumulationDistributionLineByTuple : AccumulationDistributionLine<(decimal High, decimal Low, decimal Close, decimal Volume), decimal?>
    {
        public AccumulationDistributionLineByTuple(IEnumerable<(decimal High, decimal Low, decimal Close, decimal Volume)> inputs) 
            : base(inputs, c => c, (c, otm) => otm)
        {
        }
    }

    public class AccumulationDistributionLine : AccumulationDistributionLine<Candle, AnalyzableTick<decimal?>>
    {
        public AccumulationDistributionLine(IEnumerable<Candle> inputs) 
            : base(inputs, c => (c.High, c.Low, c.Close, c.Volume), (c, otm) => new AnalyzableTick<decimal?>(c.DateTime, otm))
        {
        }
    }
}