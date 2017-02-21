using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;
using Trady.Core.Helper;

namespace Trady.Analysis
{
    public abstract class AnalyzableBase<TTick> : IAnalyzable where TTick : ITick
    {
        public AnalyzableBase(Equity equity)
        {
            Equity = equity;
        }

        public Equity Equity { get; private set; }

        public TimeSeries<TTick> Compute(DateTime? startTime, DateTime? endTime)
        {
            var ticks = new List<TTick>();

            int startIndex = ComputeStartIndex(startTime);
            int endIndex = ComputeEndIndex(endTime);

            for (int i = startIndex; i <= endIndex; i++)
                ticks.Add(ComputeByIndex(i));

            return new TimeSeries<TTick>(Equity.Name, ticks, Equity.Period, Equity.MaxCount);
        }

        public TTick ComputeByDateTime(DateTime dateTime)
        {
            int? index = Equity.ToList().FindLastIndexOrDefault(c => c.DateTime <= dateTime);
            return index.HasValue ? ComputeByIndex(index.Value) : default(TTick);
        }

        public virtual TTick ComputeByIndex(int index) => ComputeByIndexImpl(index);

        protected abstract TTick ComputeByIndexImpl(int index);

        protected virtual int ComputeStartIndex(DateTime? startTime)
            => startTime.HasValue ? Equity.ToList().FindIndexOrDefault(c => c.DateTime >= startTime) ?? 0 : 0;

        protected virtual int ComputeEndIndex(DateTime? endTime)
            => endTime.HasValue ? Equity.ToList().FindLastIndexOrDefault(c => c.DateTime < endTime) ?? Equity.Count - 1 : Equity.Count - 1;
    }
}