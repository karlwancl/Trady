using System;
using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.StandardDeviation;

namespace Trady.Analysis.Indicator
{
    public partial class StandardDeviation : IndicatorBase<IndicatorResult>
    {
        public StandardDeviation(Equity equity, int periodCount) : base(equity, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            if (index < PeriodCount - 1)
                return new IndicatorResult(Equity[index].DateTime, null);

            var values = Equity.Skip(index - PeriodCount + 1).Take(PeriodCount).Select(c => c.Close);
            decimal avg = values.Average();
            double diffSum = values.Select(v => Convert.ToDouble((v - avg) * (v - avg))).Sum();
            decimal sd = Convert.ToDecimal(Math.Sqrt(diffSum / (values.Count() - 1)));

            return new IndicatorResult(Equity[index].DateTime, sd);
        }
    }
}