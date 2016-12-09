using System;
using System.Collections.Generic;

namespace Trady.Analysis.Pattern
{
    public abstract class PatternResultBase : AnalyticResultBase<bool>
    {
        protected PatternResultBase(DateTime dateTime, IDictionary<string, bool> values) : base(dateTime, values)
        {
        }
    }
}
