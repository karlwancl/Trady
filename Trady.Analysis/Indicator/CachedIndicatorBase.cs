using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public abstract class CachedIndicatorBase : IndicatorBase
    {
        private IList<IAnalyticResult<decimal>> _cache;

        public CachedIndicatorBase(Equity equity, params int[] parameters) : base(equity, parameters)
        {
            _cache = new List<IAnalyticResult<decimal>>();
        }

        protected long CacheSize { get; set; } = 256;

        protected TIndicatorResult GetComputed<TIndicatorResult>(int index) where TIndicatorResult: IAnalyticResult<decimal>
        {
            var candleDateTime = Equity[index].DateTime;

            var item = _cache.FirstOrDefault(r => r.DateTime.Equals(candleDateTime));
            if (item == null)
                item = ComputeResultByIndex<TIndicatorResult>(index);

            return (TIndicatorResult)item;
        }

        protected void CacheComputed(IAnalyticResult<decimal> result)
        {
            var item = _cache.FirstOrDefault(r => r.DateTime.Equals(result.DateTime));
            if (item != null)
                _cache.Remove(item);

            if (_cache.Count > CacheSize)
            {
                for (int i = 0; i < _cache.Count - CacheSize + 1; i++)
                    _cache.RemoveAt(0);
            }

            _cache.Add(result);
        }
    }
}
