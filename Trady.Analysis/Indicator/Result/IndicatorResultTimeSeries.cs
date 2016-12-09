using System.Collections.Generic;
using Trady.Core.Period;

namespace Trady.Analysis.Indicator
{
    public class IndicatorResultTimeSeries<TIndicatorResult> : AnalyticResultTimeSeries<TIndicatorResult, decimal>
        where TIndicatorResult: IAnalyticResult<decimal>
    {
        public IndicatorResultTimeSeries(string name, IList<TIndicatorResult> ticks, PeriodOption period, int maxTickCount) 
            : base(name, ticks, period, maxTickCount)
        {
        }
    }
}
