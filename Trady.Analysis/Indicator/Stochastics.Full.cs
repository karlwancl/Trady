using System;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class Full : IndicatorBase<IndicatorResult>
        {
            private Fast _fastStochasticsIndicator;

            public Full(Equity equity, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : base(equity, periodCount, smaPeriodCountK, smaPeriodCountD)
            {
                _fastStochasticsIndicator = new Fast(equity, periodCount, smaPeriodCountK);
            }

            public int PeriodCount => Parameters[0];

            public int SmaPeriodCountK => Parameters[1];

            public int SmaPeriodCountD => Parameters[2];

            protected override IndicatorResult ComputeByIndexImpl(int index)
            {
                var d = _fastStochasticsIndicator.ComputeByIndex(index).D;
                Func<int, decimal?> dFunc = i => _fastStochasticsIndicator.ComputeByIndex(index - SmaPeriodCountD + i + 1).D;
                decimal? dAvg = index >= SmaPeriodCountK - 1 ? Enumerable.Range(0, SmaPeriodCountD).Average(i => dFunc(i)) : null;
                return new IndicatorResult(Equity[index].DateTime, d, dAvg, 3 * d - 2 * dAvg);
            }
        }
    }
}