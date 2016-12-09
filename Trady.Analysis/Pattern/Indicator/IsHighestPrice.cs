using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsHighestPrice : PatternBase<NonDirectionalPatternResult>
    {
        private int _periodCount;

        public IsHighestPrice(Equity series, int periodCount) : base(series)
        {
            _periodCount = periodCount;
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            bool isHighest = Series.Skip(Series.Count - _periodCount).Max(c => c.Close) == Series[index].Close;
            return new NonDirectionalPatternResult(Series[index].DateTime, isHighest);
        }
    }
}
