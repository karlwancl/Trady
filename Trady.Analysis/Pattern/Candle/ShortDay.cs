using System;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candle
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class ShortDay : AnalyzableBase<PatternResult<Match?>>
    {
        public ShortDay(Equity equity) : base(equity)
        {
        }

        protected override PatternResult<Match?> ComputeByIndexImpl(int index)
        {
            throw new NotImplementedException();
        }
    }
}