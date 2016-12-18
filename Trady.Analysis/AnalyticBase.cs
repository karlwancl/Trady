using System;
using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Helper;
using System.Linq;

namespace Trady.Analysis
{
    public abstract class AnalyticBase<TTick> : IAnalytic where TTick : ITick
    {
        public AnalyticBase(Equity equity)
        {
            Equity = equity;
        }

        public Equity Equity { get; private set; }

        public TimeSeries<TTick> Compute(DateTime? startTime, DateTime? endTime)
        {
            var ticks = new List<TTick>();

            int startIndex = GetStartIndex(startTime);
            int endIndex = GetEndIndex(endTime);

            for (int i = startIndex; i <= endIndex; i++)
                ticks.Add(ComputeByIndex(i));

            return new TimeSeries<TTick>(Equity.Name, ticks, Equity.Period, Equity.MaxCount);
        }

        public TTick ComputeByDateTime(DateTime dateTime)
        {
            int? index = Equity.ToList().FindLastIndexOrDefault(c => c.DateTime <= dateTime);
            return index.HasValue ? ComputeByIndex(index.Value) : default(TTick);
        }

        public abstract TTick ComputeByIndex(int index);

        protected virtual int GetStartIndex(DateTime? startTime)
            => startTime.HasValue ? Equity.ToList().FindIndexOrDefault(c => c.DateTime >= startTime) ?? 0 : 0;

        protected virtual int GetEndIndex(DateTime? endTime)
            => endTime.HasValue ? Equity.ToList().FindLastIndexOrDefault(c => c.DateTime < endTime) ?? Equity.Count - 1 : Equity.Count - 1;
    }
}
