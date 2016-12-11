using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsHighestPrice : PatternBase<IsMatchedResult>
    {
        private int _periodCount;

        public IsHighestPrice(Equity equity, int periodCount) : base(equity)
        {
            _periodCount = periodCount;
        }

        protected override TickBase ComputeResultByIndex(int index)
        {
            bool isHighest = Equity.Skip(Equity.TickCount - _periodCount).Max(c => c.Close) == Equity[index].Close;
            return new IsMatchedResult(Equity[index].DateTime, isHighest);
        }
    }
}
