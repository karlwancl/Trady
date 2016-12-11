using System;
using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Helper;

namespace Trady.Analysis
{
    public abstract class AnalyticBase
    {
        public AnalyticBase(Equity equity)
        {
            Equity = equity;
        }

        protected Equity Equity { get; private set; }

        protected IList<TAnalyticResult> ComputeResults<TAnalyticResult>(DateTime? startTime, DateTime? endTime)
            where TAnalyticResult : TickBase
        {
            var results = new List<TAnalyticResult>();

            int startIndex = startTime == null ? 0 : Equity.FindFirstIndexOrDefault(c => c.DateTime >= startTime) ?? 0;
            int endIndex = endTime == null ? Equity.TickCount - 1 : Equity.FindLastIndexOrDefault(c => c.DateTime < endTime) ?? Equity.TickCount - 1;

            for (int i = startIndex; i <= endIndex; i++)
                results.Add(ComputeResultByIndex<TAnalyticResult>(i));

            return results;
        }

        protected TAnalyticResult ComputeResultByDateTime<TAnalyticResult>(DateTime dateTime) where TAnalyticResult: TickBase
        {
            int? index = Equity.FindLastIndexOrDefault(c => c.DateTime <= dateTime);
            if (!index.HasValue)
                return default(TAnalyticResult);
            return ComputeResultByIndex<TAnalyticResult>(index.Value);
        }

        protected TAnalyticResult ComputeResultByIndex<TAnalyticResult>(int index) where TAnalyticResult : TickBase
            => (TAnalyticResult)ComputeResultByIndex(index);

        protected abstract TickBase ComputeResultByIndex(int index);
    }
}
