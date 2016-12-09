using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsLowestPrice : PatternBase<NonDirectionalPatternResult>
    {
        private int _periodCount;

        public IsLowestPrice(Equity series, int periodCount) : base(series)
        {
            _periodCount = periodCount;
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            bool isLowest = Series.Skip(Series.Count - _periodCount).Min(c => c.Close) == Series[index].Close;
            return new NonDirectionalPatternResult(Series[index].DateTime, isLowest);
        }
    }
}
