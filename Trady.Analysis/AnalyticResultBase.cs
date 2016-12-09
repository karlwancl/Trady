using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis
{
    public abstract class AnalyticResultBase<TValueType> : TickBase, IAnalyticResult<TValueType>, IAnalyticResult
    {
        private IDictionary<string, TValueType> _values;

        protected AnalyticResultBase(DateTime dateTime, IDictionary<string, TValueType> values) : base(dateTime)
        {
            _values = values;
        }

        public IDictionary<string, TValueType> Values => _values;

        IDictionary<string, object> IAnalyticResult.Values => _values.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
    }
}
