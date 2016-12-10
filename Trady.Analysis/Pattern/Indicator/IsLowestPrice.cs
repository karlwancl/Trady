using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsLowestPrice : PatternBase<IsMatchedResult>
    {
        private int _periodCount;

        public IsLowestPrice(Equity equity, int periodCount) : base(equity)
        {
            _periodCount = periodCount;
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            bool isLowest = Equity.Skip(Equity.Count - _periodCount).Min(c => c.Close) == Equity[index].Close;
            return new IsMatchedResult(Equity[index].DateTime, isLowest);
        }
    }
}
