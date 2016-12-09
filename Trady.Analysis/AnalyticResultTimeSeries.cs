using System.Collections.Generic;
using System.Linq;
using Trady.Core;
using Trady.Core.Period;

namespace Trady.Analysis
{
    public class AnalyticResultTimeSeries<TAnalyticResult, TValueType> : TimeSeriesBase<TAnalyticResult>, IAnalyticResultTimeSeries<TValueType>, IAnalyticResultTimeSeries
        where TAnalyticResult : IAnalyticResult<TValueType>
    {
        public AnalyticResultTimeSeries(string name, IList<TAnalyticResult> ticks, PeriodOption period, int maxTickCount) 
            : base(name, ticks, period, maxTickCount)
        {
        }

        IAnalyticResult ITimeSeries<IAnalyticResult>.this[int index] => (IAnalyticResult)base[index];

        IAnalyticResult<TValueType> ITimeSeries<IAnalyticResult<TValueType>>.this[int index] => base[index];

        public void Add(IAnalyticResult tick) => base.Add((TAnalyticResult)tick);

        public void Add(IAnalyticResult<TValueType> tick) => base.Add((TAnalyticResult)tick);

        IEnumerator<IAnalyticResult> IEnumerable<IAnalyticResult>.GetEnumerator() => Ticks.Select(t => (IAnalyticResult)t).GetEnumerator();

        IEnumerator<IAnalyticResult<TValueType>> IEnumerable<IAnalyticResult<TValueType>>.GetEnumerator() => Ticks.Select(t => (IAnalyticResult<TValueType>)t).GetEnumerator();
    }
}
