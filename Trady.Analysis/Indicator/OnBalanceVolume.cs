using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class OnBalanceVolume : CumulativeAnalyzableBase<(decimal Close, decimal Volume), decimal?>
    {
        public OnBalanceVolume(IList<Candle> candles)
            : this(candles.Select(c => (c.Close, c.Volume)).ToList())
        {
        }

        public OnBalanceVolume(IList<(decimal Close, decimal Volume)> inputs) : base(inputs)
        {
        }

        protected override int InitialValueIndex => 0;

        protected override decimal? ComputeNullValue(int index) => null;

        protected override decimal? ComputeInitialValue(int index) => Inputs[index].Volume;

        protected override decimal? ComputeCumulativeValue(int index, decimal? prevOutput)
        {
            var input = Inputs[index];
            var prevInput = Inputs[index - 1];
            decimal increment = input.Volume * (input.Close > prevInput.Close ? 1 : (input.Close == prevInput.Close ? 0 : -1));
            return prevOutput + increment;
        }
    }
}