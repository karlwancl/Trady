using Trady.Core;

namespace Trady.Analysis
{
    public interface IAnalyticResultTimeSeries : IFixedTimeSeries<IAnalyticResult>
    {

    }

    public interface IAnalyticResultTimeSeries<TValueType> : IFixedTimeSeries<IAnalyticResult<TValueType>>
    {

    }
}
