using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public abstract class CachedIndicatorBase : IndicatorBase
    {
        private IList<IAnalyticResult<decimal>> _results;

        protected CachedIndicatorBase(Equity series, params int[] parameters) : base(series, parameters)
        {
            _results = new List<IAnalyticResult<decimal>>();
        }

        protected long MaxCacheCount { get; set; } = 256;

        protected TIndicatorResult GetComputed<TIndicatorResult>(int index) where TIndicatorResult: IAnalyticResult<decimal>
        {
            var candleDateTime = Series[index].DateTime;

            var result = _results.FirstOrDefault(r => r.DateTime.Equals(candleDateTime));
            if (result == null)
                result = ComputeResultByIndex<TIndicatorResult>(index);

            return (TIndicatorResult)result;
        }

        protected void CacheComputed(IAnalyticResult<decimal> result)
        {
            var resultInCache = _results.FirstOrDefault(r => r.DateTime.Equals(result.DateTime));
            if (resultInCache != null)
                _results.Remove(resultInCache);

            if (_results.Count > MaxCacheCount)
            {
                for (int i = 0; i < _results.Count - MaxCacheCount + 1; i++)
                    _results.RemoveAt(0);
            }

            _results.Add(result);
        }
    }
}
