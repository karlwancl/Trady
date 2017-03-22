using Trady.Core;
using static Trady.Analysis.Indicator.AroonOscillator;

namespace Trady.Analysis.Indicator
{
    public partial class AroonOscillator : IndicatorBase<IndicatorResult>
    {
        private Aroon _aroonIndicator;

        public AroonOscillator(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _aroonIndicator = new Aroon(equity, periodCount);
        }

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            var aroon = _aroonIndicator.ComputeByIndex(index);
            return new IndicatorResult(Equity[index].DateTime, aroon.Up - aroon.Down);
        }
    }
}