using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsHighestPrice : IndicatorBase<PatternResult<Match?>>
    {
        public IsHighestPrice(Equity equity, int periodCount)
            : base(equity, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override PatternResult<Match?> ComputeByIndexImpl(int index)
        {
            bool isHighest = Equity.Skip(Equity.Count - PeriodCount).Max(c => c.Close) == Equity[index].Close;
            return new PatternResult<Match?>(Equity[index].DateTime, Decision.IsMatch(isHighest));
        }
    }
}