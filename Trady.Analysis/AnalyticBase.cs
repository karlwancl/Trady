using System;
using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Helper;

namespace Trady.Analysis
{
    public abstract class AnalyticBase<TTick> where TTick : ITick
    {
        public AnalyticBase(Equity equity)
        {
            Equity = equity;
        }

        protected Equity Equity { get; private set; }

        public TimeSeries<TTick> Compute(DateTime? startTime, DateTime? endTime)
        {
            var ticks = new List<TTick>();

            int startIndex = startTime.HasValue ? Equity.FindFirstIndexOrDefault(c => c.DateTime >= startTime) ?? 0 : 0;
            int endIndex = endTime.HasValue ? Equity.FindLastIndexOrDefault(c => c.DateTime < endTime) ?? Equity.TickCount - 1 : Equity.TickCount - 1;

            for (int i = startIndex; i <= endIndex; i++)
                ticks.Add(ComputeByIndex(i));

            return new TimeSeries<TTick>(Equity.Name, ticks, Equity.Period, Equity.MaxTickCount);
        }

        public TTick ComputeByDateTime(DateTime dateTime)
        {
            int? index = Equity.FindLastIndexOrDefault(c => c.DateTime <= dateTime);
            return index.HasValue ? ComputeByIndex(index.Value) : default(TTick);
        }

        public abstract TTick ComputeByIndex(int index);
    }
}
