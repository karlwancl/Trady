using System;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candle
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class Doji : AnalyzableBase<IsMatchedResult>
    {
        public Doji(Equity equity) : base(equity)
        {
        }

        protected override IsMatchedResult ComputeByIndexImpl(int index)
        {
            throw new NotImplementedException();
        }
    }
}