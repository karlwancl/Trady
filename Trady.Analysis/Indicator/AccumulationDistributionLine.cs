using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AccumulationDistributionLine : CumulativeAnalyzableBase<(decimal High, decimal Low, decimal Close, decimal Volume), decimal?>
    {
        public AccumulationDistributionLine(IList<Candle> candles) :
            this(candles.Select(i => (i.High, i.Low, i.Close, i.Volume)).ToList())
        {
        }

        public AccumulationDistributionLine(IList<(decimal High, decimal Low, decimal Close, decimal Volume)> inputs) : base(inputs)
        {
        }

        protected override int InitialValueIndex => 0;

        protected override decimal? ComputeNullValue(int index) => null;

        protected override decimal? ComputeInitialValue(int index) => Inputs[index].Volume;

        protected override decimal? ComputeCumulativeValue(int index, decimal? prevOutput)
        {
            var input = Inputs[index];
            var prevInput = Inputs[index - 1];

            decimal ratio = (input.High == input.Low) ?
                (input.Close / prevInput.Close) - 1 :
                (input.Close * 2 - input.Low - input.High) / (input.High - input.Low);

            return prevOutput + ratio * input.Volume;
        }
    }
}