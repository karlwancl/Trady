using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class KaufmanAdaptiveMovingAverage : AnalyzableBase<decimal, decimal?>
    {
        private EfficiencyRatio _er;
        private GenericExponentialMovingAverage<decimal> _gema;

        public KaufmanAdaptiveMovingAverage(IList<Candle> candles, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount) :
            this(candles.Select(c => c.Close).ToList(), periodCount, emaFastPeriodCount, emaSlowPeriodCount)
        {
        }

        public KaufmanAdaptiveMovingAverage(IList<decimal> closes, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount)
            : base(closes)
        {
            _er = new EfficiencyRatio(closes, periodCount);

            Func<int, decimal> sc = i =>
            {
                double erValue = Convert.ToDouble(_er[i]);
                return Convert.ToDecimal(Math.Pow(erValue * (2.0 / (emaFastPeriodCount + 1) - 2.0 / (emaSlowPeriodCount + 1)) + 2.0 / (emaSlowPeriodCount + 1), 2));
            };

            _gema = new GenericExponentialMovingAverage<decimal>(
                closes,
                periodCount - 1,
                i => Inputs[i],
                i => Inputs[i],
                i => sc(i));

            PeriodCount = periodCount;
            EmaFastPeriodCount = emaFastPeriodCount;
            EmaSlowPeriodCount = emaSlowPeriodCount;
        }

        public int PeriodCount { get; private set; }

        public int EmaFastPeriodCount { get; private set; }

        public int EmaSlowPeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index) => _gema[index];
    }
}