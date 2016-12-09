using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrength : CachedIndicatorBase
    {
        private const string GainTag = "Gain";
        private const string LossTag = "Loss";
        private const string RsTag = "Rs";

        private ClosePriceChange _closePriceChangeIndicator;

        public RelativeStrength(Equity series, int periodCount) : base(series, periodCount)
        {
            _closePriceChangeIndicator = new ClosePriceChange(series);
        }

        public int PeriodCount => Parameters[0];

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            var change = _closePriceChangeIndicator.ComputeByIndex(index).Change;
            var tuple = (Gain: 0m, Loss: 0m, Rs: 0m);
            if (index < PeriodCount)
                tuple.Rs = 0;
            else if (index == PeriodCount)
            {
                for (int i = 0; i < PeriodCount; i++)
                {
                    decimal iChange = _closePriceChangeIndicator.ComputeByIndex(index + i + 1).Change;
                    if (iChange > 0) tuple.Gain += iChange / PeriodCount; else tuple.Loss += -iChange / PeriodCount;
                    tuple.Rs = tuple.Gain / tuple.Loss;
                }
            }
            else
            {
                var prevRsResult = GetComputed<IndicatorResult>(index - 1);
                tuple.Gain = (prevRsResult.Gain * (PeriodCount - 1) + (change > 0 ? change : 0)) / PeriodCount;
                tuple.Loss = (prevRsResult.Loss * (PeriodCount - 1) + (change < 0 ? -change : 0)) / PeriodCount;
                tuple.Rs = tuple.Gain / tuple.Loss;
            }

            var result = new IndicatorResult(Series[index].DateTime, tuple.Gain, tuple.Loss, tuple.Rs);
            CacheComputed(result);
            return result;
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Series.Name, ComputeResults<IndicatorResult>(startTime, endTime), Series.Period, Series.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
