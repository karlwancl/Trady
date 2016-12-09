using System;
using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Helper;

namespace Trady.Analysis
{
    public abstract class AnalyticBase<TValueType>
    {
        public AnalyticBase(Equity series)
        {
            Series = series;
        }

        protected Equity Series { get; private set; }

        protected IList<TAnalyticResult> ComputeResults<TAnalyticResult>(DateTime? startTime, DateTime? endTime)
            where TAnalyticResult : IAnalyticResult<TValueType>
        {
            var results = new List<TAnalyticResult>();

            int startIndex = startTime == null ? 0 : Series.FindFirstIndexOrDefault(c => c.DateTime >= startTime) ?? 0;
            int endIndex = endTime == null ? Series.Count - 1 : Series.FindLastIndexOrDefault(c => c.DateTime < endTime) ?? Series.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                results.Add(ComputeResultByIndex<TAnalyticResult>(i));

            return results;
        }

        protected TAnalyticResult ComputeResultByDateTime<TAnalyticResult>(DateTime dateTime) where TAnalyticResult: IAnalyticResult<TValueType>
        {
            int? index = Series.FindLastIndexOrDefault(c => c.DateTime <= dateTime);
            if (!index.HasValue)
                return default(TAnalyticResult);
            return ComputeResultByIndex<TAnalyticResult>(index.Value);
        }

        protected TAnalyticResult ComputeResultByIndex<TAnalyticResult>(int index) where TAnalyticResult : IAnalyticResult<TValueType>
            => (TAnalyticResult)ComputeResultByIndex(index);

        protected abstract IAnalyticResult<TValueType> ComputeResultByIndex(int index);
    }
}
