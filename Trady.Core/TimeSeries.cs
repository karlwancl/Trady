using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Exception;
using Trady.Core.Helper;
using Trady.Core.Period;

namespace Trady.Core
{
    public class TimeSeries<TTick> : ITimeSeries<TTick> where TTick : ITick
    {
        public TimeSeries(string name, IList<TTick> ticks, PeriodOption period, int maxTickCount)
        {
            Name = name;
            Ticks = (ticks ?? new List<TTick>()).OrderBy(t => t.DateTime).ToList();
            Period = period;
            MaxCount = maxTickCount;

            // Check if time frame is overlapped before construction finishes
            if (IsTimeFrameOverlapped(Ticks, period, out var tick))
                throw new InvalidTimeFrameException(tick.DateTime);
        }

        public string Name { get; private set; }

        protected IList<TTick> Ticks { get; private set; }

        IList<ITick> ITimeSeries.Ticks => Ticks.Select(t => (ITick)t).ToList();

        public PeriodOption Period { get; private set; }

        private int _maxCount;

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

        public int Count => Ticks.Count;

        public bool IsReadOnly => Ticks.IsReadOnly;

        public TTick this[int index] { get => Ticks[index]; set => throw new InvalidOperationException(); }

        public int IndexOf(TTick item) => Ticks.IndexOf(item);

        public void Insert(int index, TTick item) => throw new InvalidOperationException();

        public void RemoveAt(int index) => throw new InvalidOperationException();

        public void Add(TTick item)
        {
            var periodInstance = Period.CreateInstance();
            if (Ticks.Any())
            {
                var tickEndTime = periodInstance.NextTimestamp(Ticks.Last().DateTime);
                if (item.DateTime <= tickEndTime)
                    throw new InvalidTimeFrameException(item.DateTime);

                if (Ticks.Count >= MaxCount)
                    for (int i = 0; i < Ticks.Count - MaxCount + 1; i++)
                        Ticks.RemoveAt(0);
            }
            Ticks.Add(item);
        }

        public void Clear() => Ticks.Clear();

        public bool Contains(TTick item) => Ticks.Contains(item);

        public void CopyTo(TTick[] array, int arrayIndex) => Ticks.CopyTo(array, arrayIndex);

        public bool Remove(TTick item) => throw new InvalidOperationException();

        public IEnumerator<TTick> GetEnumerator() => Ticks.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Ticks.GetEnumerator();

        private static bool IsTimeFrameOverlapped<TConstraintTick>(IList<TConstraintTick> ticks, PeriodOption period, out TConstraintTick tick)
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
