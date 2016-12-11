using System;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBands : IndicatorBase
    {
        private SimpleMovingAverage _smaIndicator;

        public BollingerBands(Equity equity, int periodCount, int sdCount) : base(equity, periodCount, sdCount)
        {
            _smaIndicator = new SimpleMovingAverage(equity, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public int SdCount => Parameters[1];

        protected override TickBase ComputeResultByIndex(int index)
        {
            decimal middleBand = _smaIndicator.ComputeByIndex(index).Sma;
            decimal sd = index >= PeriodCount - 1 ? Equity.Skip(index - PeriodCount + 1).Take(PeriodCount).Select(c => c.Close).Sd() : 0;
            return new IndicatorResult(Equity[index].DateTime, middleBand - SdCount * sd, middleBand, middleBand + SdCount * sd, 2 * SdCount * sd);
        }

        public TimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
           => new TimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
