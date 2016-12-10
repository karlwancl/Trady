using System;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrength : IndicatorBase
    {
        private const string GainTag = "Gain";
        private const string LossTag = "Loss";
        private const string RsTag = "Rs";

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

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            if (index < PeriodCount - 1)
                return new IndicatorResult(Equity[index].DateTime, 0);

            decimal gain = _uEma.Compute(index);
            decimal loss = _dEma.Compute(index);
            return new IndicatorResult(Equity[index].DateTime, gain / loss);
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
