using Trady.Core;
using static Trady.Analysis.Indicator.BollingerBands;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBands : IndicatorBase<IndicatorResult>
    {
        private SimpleMovingAverage _smaIndicator;
        private StandardDeviation _sdIndicator;

        public BollingerBands(Equity equity, int periodCount, int sdCount) : base(equity, periodCount, sdCount)
        {
            _smaIndicator = new SimpleMovingAverage(equity, periodCount);
            _sdIndicator = new StandardDeviation(equity, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public int SdCount => Parameters[1];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            decimal? middleBand = _smaIndicator.ComputeByIndex(index).Sma;
            decimal? sd = _sdIndicator.ComputeByIndex(index).Sd;
            return new IndicatorResult(Equity[index].DateTime, middleBand - SdCount * sd, middleBand, middleBand + SdCount * sd);
        }
    }
}