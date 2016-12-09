using System;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candle
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class UpsideTasukiGap : PatternBase<DirectionalPatternResult>
    {
        public UpsideTasukiGap(Equity series) : base(series)
        {
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            throw new NotImplementedException();
        }
    }
}
