using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Analysis.Indicator.Helper
{
    internal class Ema
    {
        private Func<int, DateTime> _dateTimeFunction;
        private Func<int, decimal> _indexedFunction;
        private int _periodCount;
        private IDictionary<DateTime, decimal> _cache;
        private decimal? _defaultValue;
        private bool _modified;

        public Ema(Func<int, DateTime> dateTimeFunction, Func<int, decimal> indexedFunction, int periodCount, decimal? defaultValue = null, bool modified = false)
        {
            _dateTimeFunction = dateTimeFunction;
            _indexedFunction = indexedFunction;
            _periodCount = periodCount;
            _defaultValue = defaultValue;
            _modified = modified;
            _cache = new Dictionary<DateTime, decimal>();
        }

        public Func<int, DateTime> DateTimeFunction => _dateTimeFunction;

        public Func<int, decimal> IndexedFunction => _indexedFunction;

        public decimal SmoothingFactor => _modified ? 1.0m / _periodCount : 2.0m / (_periodCount + 1);
        
        public decimal Compute(int index)
        {
            var currDateTime = _dateTimeFunction(index);
            if (_cache.TryGetValue(currDateTime, out decimal v))
                return v;

            decimal value;
            if (index == 0)
                value = _defaultValue.HasValue ? 0 : _indexedFunction(index);
            else
            {
                if (_defaultValue.HasValue && index < _periodCount)
                    value = index == _periodCount - 1 ? _defaultValue.Value : 0;
                else
                {
                    var prevDateTime = _dateTimeFunction(index - 1);
                    var prevValue = _cache.ContainsKey(prevDateTime) ? _cache[prevDateTime] : Compute(index - 1);
                    value = prevValue + (SmoothingFactor * (_indexedFunction(index) - prevValue));
                }
            }
            if (!_cache.ContainsKey(currDateTime))
                _cache.Add(new KeyValuePair<DateTime, decimal>(currDateTime, value));
            return value;
        }
    }
}
