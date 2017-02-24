using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageOscillatorTrend : IndicatorBase<PatternResult<Trend?>>
    {
        private ExponentialMovingAverageOscillator _emaOscillator;

        public ExponentialMovingAverageOscillatorTrend(Equity equity, int periodCount1, int periodCount2)
            : base(equity, periodCount1, periodCount2)
        {
            _emaOscillator = new ExponentialMovingAverageOscillator(equity, periodCount1, periodCount2);
        }

        protected override PatternResult<Trend?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new PatternResult<Trend?>(Equity[index].DateTime, null);

            var latest = _emaOscillator.ComputeByIndex(index);
            var secondLatest = _emaOscillator.ComputeByIndex(index - 1);

            return new PatternResult<Trend?>(Equity[index].DateTime, Decision.IsTrending(latest.Osc - secondLatest.Osc));
        }
    }
}
