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

            protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
            {
                decimal rsv = _rsvIndicator.ComputeByIndex(index).Rsv;
                var candle = Equity[index];

                var tuple = (K: 50m, D: 50m, J: 50m);
                if (index >= SmaPeriodCount - 1)
                {
                    decimal sum = 0;
                    for (int i = 0; i < SmaPeriodCount; i++)
                    {
                        sum += _rsvIndicator.ComputeByIndex(index - SmaPeriodCount + i + 1).Rsv;
                    }
                    tuple = (rsv, sum / SmaPeriodCount, 3 * rsv - 2 * sum / SmaPeriodCount);
                }

                return new IndicatorResult(candle.DateTime, tuple.K, tuple.D, tuple.J);
            }
        }
    }
}
