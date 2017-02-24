using System.Linq;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsLowestPrice : IndicatorBase<PatternResult<Match?>>
    {
        public IsLowestPrice(Equity equity, int periodCount)
            : base(equity, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override PatternResult<Match?> ComputeByIndexImpl(int index)
        {
            bool isLowest = Equity.Skip(Equity.Count - PeriodCount).Min(c => c.Close) == Equity[index].Close;
            return new PatternResult<Match?>(Equity[index].DateTime, Decision.IsMatch(isLowest));
        }
    }
}