using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis
{
    public interface IAnalyticResult : ITick
    {
        IDictionary<string, object> Values { get; }
    }

    public interface IAnalyticResult<TValueType> : ITick
    {
        IDictionary<string, TValueType> Values { get; }
    }
}
