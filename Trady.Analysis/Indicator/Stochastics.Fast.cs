using System;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class Fast : IndicatorBase
        {
            private RawStochasticsValue _rsvIndicator;

            public Fast(Equity equity, int periodCount, int smaPeriodCount) 
                : base(equity, periodCount, smaPeriodCount, 0)
            {
                _rsvIndicator = new RawStochasticsValue(equity, periodCount);
            }

            public int PeriodCount => Parameters[0];

            public int SmaPeriodCount => Parameters[1];

            protected override TickBase ComputeResultByIndex(int index)
            {
                decimal rsv = _rsvIndicator.ComputeByIndex(index).Rsv;

                Func<int, decimal> rsvFunc = i => _rsvIndicator.ComputeByIndex(index - SmaPeriodCount + i + 1).Rsv;
                decimal rsvAvg = index >= SmaPeriodCount - 1 ? Enumerable.Range(0, SmaPeriodCount).Average(i => rsvFunc(i)) : 0;
                return new IndicatorResult(Equity[index].DateTime, rsv, rsvAvg, 3 * rsv - 2 * rsvAvg);
            }
        }
    }
}
