using System;
using Trady.Core;

namespace Trady.Analysis.Pattern
{
    public abstract class PatternBase<TPatternResult> : AnalyticBase
        where TPatternResult: TickBase
    {
        protected PatternBase(Equity equity) : base(equity)
        {
        }

        public TimeSeries<TPatternResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new TimeSeries<TPatternResult>(Equity.Name, ComputeResults<TPatternResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public TPatternResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<TPatternResult>(dateTime);

        public TPatternResult ComputeByIndex(int index)
            => ComputeResultByIndex<TPatternResult>(index);
    }
}
