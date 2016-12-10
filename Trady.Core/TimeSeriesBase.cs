using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Helper;
using Trady.Core.Period;
using Trady.Logging;

namespace Trady.Core
{
    public abstract class TimeSeriesBase<TTick> : IFixedTimeSeries<TTick> where TTick : ITick
    {
        private IList<TTick> _ticks;

        private int _maxTickCount;

        protected TimeSeriesBase(string name ,IList<TTick> ticks, PeriodOption period, int maxTickCount)
        {
            Name = name;
            _ticks = (ticks ?? new List<TTick>()).OrderBy(t => t.DateTime).ToList();
            MaxTickCount = maxTickCount;

            Period = period;
            if (IsTimeframeOverlap(_ticks, period, out var tick))
                throw new ArgumentException($"Timeframe has overlapped: {tick.DateTime}");
        }

        public string Name { get; private set; }

        public TTick this[int index] => _ticks[index];

        public void Add(TTick tick)
        {
            var periodInstance = Period.CreateInstance();
            try
            {
                if (_ticks.Any())
                {
                    var tickEndTime = periodInstance.NextTimestamp(_ticks.Last().DateTime);
                    if (tickEndTime > tick.DateTime)
                        throw new ArgumentException("The added candle should be more recent than the latest in the series");

                    if (_ticks.Count >= MaxTickCount)
                    {
                        for (int i = 0; i < _ticks.Count - MaxTickCount + 1; i++)
                            _ticks.RemoveAt(0);
                    }
                }
                _ticks.Add(tick);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public void Clear()
        {
            _ticks.Clear();
        }

        public int Count => _ticks.Count;

        public PeriodOption Period { get; private set; }

        public int MaxTickCount
        {
            get
            {
                return _maxTickCount;
            }
            set
            {
                _maxTickCount = value;
                if (_ticks.Count > _maxTickCount)
                    _ticks = _ticks.Skip(_ticks.Count - _maxTickCount).Take(_maxTickCount).ToList();
            }
        }

        protected IList<TTick> Ticks => _ticks;

        public IEnumerator<TTick> GetEnumerator() => _ticks.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _ticks.GetEnumerator();

        private static bool IsTimeframeOverlap<TConstraintTick>(IList<TConstraintTick> ticks, PeriodOption period, out TConstraintTick tick)
            where TConstraintTick : ITick
        {
            var periodInstance = period.CreateInstance();
            tick = default(TConstraintTick);

            for (int i = 0; i < ticks.Count() - 1; i++)
            {
                var candleEndTime = periodInstance.NextTimestamp(ticks.ElementAt(i).DateTime);
                if (candleEndTime > ticks.ElementAt(i + 1).DateTime)
                {
                    tick = ticks.ElementAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}
