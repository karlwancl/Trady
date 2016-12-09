using System.Collections.Generic;
using Trady.Core.Period;

namespace Trady.Analysis.Pattern
{
    public class PatternResultTimeSeries<TPatternResult> : AnalyticResultTimeSeries<TPatternResult, bool>
        where TPatternResult: IAnalyticResult<bool>
    {
        public PatternResultTimeSeries(string name, IList<TPatternResult> ticks, PeriodOption period, int maxTickCount) 
            : base(name, ticks, period, maxTickCount)
        {
        }
    }
}
