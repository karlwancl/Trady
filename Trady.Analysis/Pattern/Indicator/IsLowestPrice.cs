using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsLowestPrice : IndicatorBase<IsMatchedResult>
    {
        public IsLowestPrice(Equity equity, int periodCount)
            : base(equity, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        public override IsMatchedResult ComputeByIndex(int index)
        {
            bool isLowest = Equity.Skip(Equity.Count - PeriodCount).Min(c => c.Close) == Equity[index].Close;
            return new IsMatchedResult(Equity[index].DateTime, isLowest);
        }
    }
}
