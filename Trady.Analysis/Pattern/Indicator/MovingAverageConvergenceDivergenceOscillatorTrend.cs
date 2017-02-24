using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class MovingAverageConvergenceDivergenceOscillatorTrend : IndicatorBase<PatternResult<Trend?>>
    {
        private MovingAverageConvergenceDivergence _macdIndicator;

        public MovingAverageConvergenceDivergenceOscillatorTrend(Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
            _macdIndicator = new MovingAverageConvergenceDivergence(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount);
        }

        protected override PatternResult<Trend?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new PatternResult<Trend?>(Equity[index].DateTime, null);

            var latest = _macdIndicator.ComputeByIndex(index);
            var secondLatest = _macdIndicator.ComputeByIndex(index - 1);

            return new PatternResult<Trend?>(Equity[index].DateTime, Decision.IsTrending(latest.Osc - secondLatest.Osc));
        }
    }
}
