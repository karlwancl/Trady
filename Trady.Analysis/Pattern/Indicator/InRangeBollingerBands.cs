using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class InRangeBollingerBands : PatternBase<NonDirectionalPatternResult>
    {
        private BollingerBands _bbIndicator;

        public InRangeBollingerBands(Equity series, int periodCount, int sdCount) : base(series)
        {
            _bbIndicator = new BollingerBands(series, periodCount, sdCount);
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            var result = _bbIndicator.ComputeByIndex(index);
            var current = Series[index];
            return new NonDirectionalPatternResult(current.DateTime, current.Close >= result.Lower && current.Close <= result.Upper);
        }
    }
}
