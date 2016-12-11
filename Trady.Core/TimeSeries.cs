using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Helper;
using Trady.Core.Period;
using Trady.Logging;

namespace Trady.Core
{
    public class TimeSeries<TTick> : IFixedTimeSeries<TTick> where TTick : ITick
    {
        private IList<ITick> _ticks;

        private int _maxTickCount;

        public TimeSeries(string name, IList<TTick> ticks, PeriodOption period, int maxTickCount)
        {
            Name = name;
            _ticks = (ticks?.Select(t => (ITick)t) ?? new List<ITick>()).OrderBy(t => t.DateTime).ToList();
            MaxTickCount = maxTickCount;

            Period = period;
            if (IsTimeframeOverlap(_ticks, period, out var tick))
                throw new ArgumentException($"Timeframe has overlapped: {tick.DateTime}");
        }

        public string Name { get; private set; }

        public void Reset()
        {
            _ticks.Clear();
        }

        public int TickCount => _ticks.Count;

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

        protected IList<TTick> Ticks => _ticks.Select(t => (TTick)t).ToList();

        ITick ITimeSeries.this[int index] => _ticks[index];

        public TTick this[int index] => (TTick)_ticks[index];

        public void Add(TTick tick) => Add((ITick)tick);

        public void Add(ITick tick)
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

        IEnumerator<TTick> IEnumerable<TTick>.GetEnumerator() => _ticks.Select(t => (TTick)t).GetEnumerator();

        public IEnumerator GetEnumerator() => _ticks.GetEnumerator();
    }
}
