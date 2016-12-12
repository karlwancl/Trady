using System;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;
using static Trady.Analysis.Indicator.BollingerBands;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBands : IndicatorBase<IndicatorResult>
    {
        private SimpleMovingAverage _smaIndicator;

        public BollingerBands(Equity equity, int periodCount, int sdCount) : base(equity, periodCount, sdCount)
        {
            _smaIndicator = new SimpleMovingAverage(equity, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public int SdCount => Parameters[1];

        public override IndicatorResult ComputeByIndex(int index)
        {
            decimal? middleBand = _smaIndicator.ComputeByIndex(index).Sma;
            decimal? sd = index >= PeriodCount - 1 ? Equity.Skip(index - PeriodCount + 1).Take(PeriodCount).Select(c => c.Close).Sd() : (decimal?)null;
            return new IndicatorResult(Equity[index].DateTime, middleBand - SdCount * sd, middleBand, middleBand + SdCount * sd, 2 * SdCount * sd);
        }
    }
}
