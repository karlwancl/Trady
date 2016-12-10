using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class Slow : IndicatorBase
        {
            const int SmaPeriodCountK = 3;

            private Full _fullStochasticsIndicator;

            public Slow(Equity equity, int periodCount, int smaPeriodCountD)
                : base(equity, periodCount, 0, smaPeriodCountD)
            {
                _fullStochasticsIndicator = new Full(equity, periodCount, SmaPeriodCountK, smaPeriodCountD);
            }

            public int PeriodCount => Parameters[0];

            public int SmaPeriodCountD => Parameters[2];

            protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
                => _fullStochasticsIndicator.ComputeByIndex(index);
        }
    }
}
