using System;
using Trady.Core;

namespace Trady.Analysis.Pattern
{
    public abstract class PatternBase<TPatternResult> : AnalyticBase<bool>
        where TPatternResult: PatternResultBase
    {
        protected PatternBase(Equity equity) : base(equity)
        {
        }

        public PatternResultTimeSeries<TPatternResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new PatternResultTimeSeries<TPatternResult>(Equity.Name, ComputeResults<TPatternResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public TPatternResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<TPatternResult>(dateTime);

        public TPatternResult ComputeByIndex(int index)
            => ComputeResultByIndex<TPatternResult>(index);
    }
}
