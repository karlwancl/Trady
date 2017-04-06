using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class StandardDeviation : IndicatorBase<decimal, decimal?>
    {
        public StandardDeviation(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public StandardDeviation(IList<decimal> closes, int periodCount) : base(closes, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override decimal? ComputeByIndexImpl(int index)
        {
            if (index < PeriodCount - 1)
                return null;

            var closes = Inputs.Skip(index - PeriodCount + 1).Take(PeriodCount);
            decimal avg = closes.Average();
            double diffSum = Convert.ToDouble(closes.Select(v => (v - avg) * (v - avg)).Sum());
            return Convert.ToDecimal(Math.Sqrt(diffSum / (closes.Count() - 1)));
        }
    }
}