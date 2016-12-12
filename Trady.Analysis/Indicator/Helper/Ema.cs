using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trady.Analysis.Indicator.Helper
{
    internal class Ema
    {
        private const int MaxStackCount = 192;
        private int _stackCount;

        private Cache<SimpleValueResult<decimal?>> _cache;

        private Func<int, DateTime> _dateTimeFunction;
        private Func<int, decimal?> _valueFunction;
        private Func<int, decimal?> _firstValueFunction;
        private int _firstValueIndex;

        private int _periodCount;
        private bool _modified;

        public Ema(Func<int, DateTime> dateTimeFunction, Func<int, decimal?> valueFunction, Func<int, decimal?> firstValueFunction, int periodCount, int firstValueIndex, bool modified = false)
        {
            _cache = new Cache<SimpleValueResult<decimal?>>();

            _dateTimeFunction = dateTimeFunction;
            _valueFunction = valueFunction;
            _firstValueFunction = firstValueFunction;

            _periodCount = periodCount;
            _firstValueIndex = firstValueIndex;
            _modified = modified;
        }

        public Func<int, DateTime> DateTimeFunction => _dateTimeFunction;

        public Func<int, decimal?> ValueFunction => _valueFunction;

        public Func<int, decimal?> FirstValueFunction => _firstValueFunction;

        public int FirstValueIndex => _firstValueIndex;

        public decimal SmoothingFactor => _modified ? 1.0m / _periodCount : 2.0m / (_periodCount + 1);
        
        public decimal? Compute(int index)
        {
            var dateTime = _dateTimeFunction(index);
            var svr = _cache.GetFromCacheOrDefault(dateTime);
            if (svr != null) return svr.Value;

            decimal? value;
            if (index < _firstValueIndex)
                value = null;
            else if (index == _firstValueIndex)
                value = _firstValueFunction(index);
            else
            {
                var prevDateTime = _dateTimeFunction(index - 1);

                decimal? prevValue;
                var prevSvr = _cache.GetFromCacheOrDefault(prevDateTime);
                if (prevSvr == null)
                {
                    if (_stackCount < MaxStackCount)
                    {
                        _stackCount++;
                        prevValue = Compute(index - 1);
                    }
                    else
                    {
                        prevValue = _firstValueFunction(index - 1);
                        _stackCount = 0;
                    }
                }
                else
                    prevValue = prevSvr.Value;

                // Nullable arithematic operation returns null if either oprand is null
                value = prevValue + (SmoothingFactor * (_valueFunction(index) - prevValue));
            }

            _cache.AddToCache(new SimpleValueResult<decimal?>(dateTime, value));
            return value;
        }
    }
}
