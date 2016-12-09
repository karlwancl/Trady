using System;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBands : IndicatorBase
    {
        private const string LowerTag = "Lower";
        private const string MiddleTag = "Middle";
        private const string UpperTag = "Upper";
        private const string WidthTag = "Width";

        private SimpleMovingAverage _smaIndicator;

        public BollingerBands(Equity series, int periodCount, int sdCount) : base(series, periodCount, sdCount)
        {
            _smaIndicator = new SimpleMovingAverage(series, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public int SdCount => Parameters[1];

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            var tuple = (LowerBand: 0m, MiddleBand: 0m, UpperBand: 0m, BandWidth: 0m);
            var smaResult = _smaIndicator.ComputeByIndex(index);

            if (index < PeriodCount - 1)
                tuple = (smaResult.Sma, smaResult.Sma, smaResult.Sma, 0);
            else
            {
                var mean = Series.Skip(index - PeriodCount + 1).Take(PeriodCount).Sum(c => c.Close) / PeriodCount;
                var sd = Convert.ToDecimal(Math.Sqrt(Series.Skip(index - PeriodCount + 1).Take(PeriodCount).Select(c => Math.Pow((double)(c.Close - mean), 2)).Sum() / (PeriodCount - 1)));
                tuple = (smaResult.Sma - SdCount * sd, smaResult.Sma, smaResult.Sma + SdCount * sd, 2 * sd * SdCount);
            }

            return new IndicatorResult(Series[index].DateTime, tuple.LowerBand, tuple.MiddleBand, tuple.UpperBand, tuple.BandWidth);
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
           => new IndicatorResultTimeSeries<IndicatorResult>(Series.Name, ComputeResults<IndicatorResult>(startTime, endTime), Series.Period, Series.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
