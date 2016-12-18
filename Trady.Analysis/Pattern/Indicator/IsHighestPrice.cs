using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsHighestPrice : IndicatorBase<IsMatchedResult>
    {
        public IsHighestPrice(Equity equity, int periodCount) 
            : base(equity, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        public override IsMatchedResult ComputeByIndex(int index)
        {
            bool isHighest = Equity.Skip(Equity.Count - PeriodCount).Max(c => c.Close) == Equity[index].Close;
            return new IsMatchedResult(Equity[index].DateTime, isHighest);
        }
    }
}
