using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AroonOscillator : AnalyzableBase<(decimal High, decimal Low), decimal?>
    {
        private Aroon _aroon;

        public AroonOscillator(IList<Candle> candles, int periodCount) :
            this(candles.Select(c => (c.High, c.Low)).ToList(), periodCount)
        {
        }

        public AroonOscillator(IList<(decimal High, decimal Low)> inputs, int periodCount) : base(inputs)
        {
            _aroon = new Aroon(inputs, periodCount);
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index)
        {
            var aroon = _aroon[index];
            return (aroon.Up - aroon.Down);
        }
    }
}