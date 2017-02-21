using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class Slow : IndicatorBase<IndicatorResult>
        {
            private const int SmaPeriodCountK = 3;

            private Full _fullStochasticsIndicator;

            public Slow(Equity equity, int periodCount, int smaPeriodCountD)
                : base(equity, periodCount, smaPeriodCountD)
            {
                _fullStochasticsIndicator = new Full(equity, periodCount, SmaPeriodCountK, smaPeriodCountD);

                RegisterDependents(_fullStochasticsIndicator);
            }

            public int PeriodCount => Parameters[0];

            public int SmaPeriodCountD => Parameters[1];

            protected override IndicatorResult ComputeByIndexImpl(int index)
                => _fullStochasticsIndicator.ComputeByIndex(index);
        }
    }
}