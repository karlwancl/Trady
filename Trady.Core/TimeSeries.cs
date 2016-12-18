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
        private int _maxCount;

        public TimeSeries(string name, IList<TTick> ticks, PeriodOption period, int maxTickCount)
        {
            Name = name;
            Ticks = (ticks ?? new List<TTick>()).OrderBy(t => t.DateTime).ToList();
            MaxCount = maxTickCount;

            Period = period;
            if (IsTimeframeOverlap(Ticks, period, out var tick))
                throw new ArgumentException($"Timeframe has overlapped: {tick.DateTime}");
        }

        public string Name { get; private set; }

        public PeriodOption Period { get; private set; }

        public int MaxCount
        {
            get
            {
                return _maxCount;
            }
            set
            {
                _maxCount = value;
                if (Ticks.Count > _maxCount)
                    Ticks = Ticks.Skip(Ticks.Count - _maxCount).Take(_maxCount).ToList();
            }
        }

        protected IList<TTick> Ticks { get; private set; }

        public int Count => Ticks.Count;

        public bool IsReadOnly => Ticks.IsReadOnly;

        IList<ITick> ITimeSeries.Ticks => Ticks.Select(t => (ITick)t).ToList();

        public TTick this[int index]
        {
            get
            {
                return Ticks[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public int IndexOf(TTick item) => Ticks.IndexOf(item);

        public void Insert(int index, TTick item) => throw new NotSupportedException();

        public void RemoveAt(int index) => throw new NotSupportedException();

        public void Add(TTick item)
        {
            var periodInstance = Period.CreateInstance();
            try
            {
                if (Ticks.Any())
                {
                    var tickEndTime = periodInstance.NextTimestamp(Ticks.Last().DateTime);
                    if (tickEndTime > item.DateTime)
                        throw new ArgumentException("The added candle should be more recent than the latest in the series");

                    if (Ticks.Count >= MaxCount)
                    {
                        for (int i = 0; i < Ticks.Count - MaxCount + 1; i++)
                            Ticks.RemoveAt(0);
                    }
                }
                Ticks.Add(item);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public void Clear() => Ticks.Clear();

        public bool Contains(TTick item) => Ticks.Contains(item);

        public void CopyTo(TTick[] array, int arrayIndex) => Ticks.CopyTo(array, arrayIndex);

        public bool Remove(TTick item) => throw new NotSupportedException();

        public IEnumerator<TTick> GetEnumerator() => Ticks.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Ticks.GetEnumerator();

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
