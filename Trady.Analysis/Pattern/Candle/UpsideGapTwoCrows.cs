using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Candle
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class UpsideGapTwoCrows : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), Match?>
    {
        public UpsideGapTwoCrows(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs) : base(inputs)
        {
        }

        protected override Match? ComputeByIndexImpl(int index)
        {
            throw new NotImplementedException();
        }
    }
}