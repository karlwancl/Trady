using System;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrength : IndicatorBase
    {
        private ClosePriceChange _closePriceChangeIndicator;
        private Ema _uEma, _dEma;

        public RelativeStrength(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _closePriceChangeIndicator = new ClosePriceChange(equity);

            _uEma = new Ema(
                i => Equity[i].DateTime,
                i =>
                {
                    var change = _closePriceChangeIndicator.ComputeByIndex(i).Change;
                    return change > 0 ? Math.Abs(change) : 0;
                },
                periodCount,
                Enumerable
                    .Range(0, PeriodCount)
                    .Select(i => _closePriceChangeIndicator.ComputeByIndex(i).Change)
                    .Average(v => v > 0 ? Math.Abs(v) : 0),
                true);

            _dEma = new Ema(
                i => Equity[i].DateTime,
                i =>
                {
                    var change = _closePriceChangeIndicator.ComputeByIndex(i).Change;
                    return change < 0 ? Math.Abs(change) : 0;
                },
                periodCount,
                Enumerable
                    .Range(0, PeriodCount)
                    .Select(i => _closePriceChangeIndicator.ComputeByIndex(i).Change)
                    .Average(v => v < 0 ? Math.Abs(v) : 0),
                true);
        }

        public int PeriodCount => Parameters[0];

        protected override TickBase ComputeResultByIndex(int index)
        {
            decimal gain = index >= PeriodCount - 1 ? _uEma.Compute(index) : 0;
            decimal loss = index >= PeriodCount - 1 ? _dEma.Compute(index) : 0;
            return new IndicatorResult(Equity[index].DateTime, loss != 0 ? gain / loss : 0);
        }

        public TimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new TimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
