using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageOscillatorTrend : IndicatorBase<PatternResult<Trend?>>
    {
        private SimpleMovingAverageOscillator _smaOscillator;

        public SimpleMovingAverageOscillatorTrend(Equity equity, int periodCount1, int periodCount2)
            : base(equity, periodCount1, periodCount2)
        {
            _smaOscillator = new SimpleMovingAverageOscillator(equity, periodCount1, periodCount2);
        }

        protected override PatternResult<Trend?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new PatternResult<Trend?>(Equity[index].DateTime, null);

            var latest = _smaOscillator.ComputeByIndex(index);
            var secondLatest = _smaOscillator.ComputeByIndex(index - 1);

            return new PatternResult<Trend?>(Equity[index].DateTime, Decision.IsTrending(latest.Osc - secondLatest.Osc));
        }
    }
}
