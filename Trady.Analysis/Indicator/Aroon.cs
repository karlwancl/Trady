using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Aroon : IndicatorBase<(decimal High, decimal Low), (decimal? Up, decimal? Down)>
    {
        private HighestHigh _hh;
        private LowestLow _ll;

        public Aroon(IList<Candle> candles, int periodCount) :
            this(candles.Select(i => (i.High, i.Low)).ToList(), periodCount)
        {
        }

        public Aroon(IList<(decimal High, decimal Low)> inputs, int periodCount) : base(inputs, periodCount)
        {
            _hh = new HighestHigh(inputs.Select(i => i.High).ToList(), periodCount);
            _ll = new LowestLow(inputs.Select(i => i.Low).ToList(), periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override (decimal? Up, decimal? Down) ComputeByIndexImpl(int index)
        {
            if (index < PeriodCount - 1)
                return (null, null);

            var nearestIndexToHighestHigh = index - PeriodCount + 1 + Inputs
                .Skip(index - PeriodCount + 1)
                .Take(PeriodCount)
                .ToList()
                .FindLastIndexOrDefault(i => i.High == _hh[index]).Value;

            var nearestIndexToLowestLow = index - PeriodCount + 1 + Inputs
                .Skip(index - PeriodCount + 1)
                .Take(PeriodCount)
                .ToList()
                .FindLastIndexOrDefault(i => i.Low == _ll[index]).Value;

            var up = 100.0m * (PeriodCount - (index - nearestIndexToHighestHigh)) / PeriodCount;
            var down = 100.0m * (PeriodCount - (index - nearestIndexToLowestLow)) / PeriodCount;

            return (up, down);
        }
    }
}