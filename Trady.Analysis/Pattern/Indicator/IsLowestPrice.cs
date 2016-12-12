using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsLowestPrice : AnalyticBase<IsMatchedResult>
    {
        private int _periodCount;

        public IsLowestPrice(Equity equity, int periodCount) : base(equity)
        {
            _periodCount = periodCount;
        }

        public override IsMatchedResult ComputeByIndex(int index)
        {
            bool isLowest = Equity.Skip(Equity.TickCount - _periodCount).Min(c => c.Close) == Equity[index].Close;
            return new IsMatchedResult(Equity[index].DateTime, isLowest);
        }
    }
}
