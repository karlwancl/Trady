using System;
using Trady.Core;

namespace Trady.Analysis.Pattern
{
    public abstract class PatternBase<TPatternResult> : AnalyticBase<bool>
        where TPatternResult: PatternResultBase
    {
        protected PatternBase(Equity series) : base(series)
        {
        }

        public PatternResultTimeSeries<TPatternResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new PatternResultTimeSeries<TPatternResult>(Series.Name, ComputeResults<TPatternResult>(startTime, endTime), Series.Period, Series.MaxTickCount);

        public TPatternResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<TPatternResult>(dateTime);

        public TPatternResult ComputeByIndex(int index)
            => ComputeResultByIndex<TPatternResult>(index);
    }
}
