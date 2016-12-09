using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trady.Analysis.Indicator
{
    public abstract class IndicatorResultBase : AnalyticResultBase<decimal>
    {
        protected IndicatorResultBase(DateTime dateTime, IDictionary<string, decimal> values) : base(dateTime, values)
        {
        }
    }
}
