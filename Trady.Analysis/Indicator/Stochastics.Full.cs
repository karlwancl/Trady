using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class Full : IndicatorBase
        {
            private Fast _fastStochasticsIndicator;

            public Full(Equity series, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : base(series, periodCount, smaPeriodCountK, smaPeriodCountD)
            {
                _fastStochasticsIndicator = new Fast(series, periodCount, smaPeriodCountK);
            }

            public int PeriodCount => Parameters[0];

            public int SmaPeriodCountK => Parameters[1];

            public int SmaPeriodCountD => Parameters[2];

            protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
            {
                var d = _fastStochasticsIndicator.ComputeByIndex(index).D;
                var tuple = (K: 50m, D: 50m, J: 50m);

                if (index >= SmaPeriodCountK - 1)
                {
                    decimal sum = 0;
                    for (int i = 0; i < SmaPeriodCountD; i++)
                    {
                        sum += _fastStochasticsIndicator.ComputeByIndex(index - SmaPeriodCountD + i + 1).D;
                    }
                    tuple = (d, sum / SmaPeriodCountD, 3 * d - 2 * sum / SmaPeriodCountD);
                }

                return new IndicatorResult(Series[index].DateTime, tuple.K, tuple.D, tuple.J);
            }
        }
    }
}
