using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Exception;
using Trady.Core.Helper;
using Trady.Core.Infrastructure;
using Trady.Core.Period;

namespace Trady.Core
{
    public class TimeSeries<TTick> : ITimeSeries<TTick> where TTick : ITick
    {
        private int? _maxTickCount = default(int?);

        public TimeSeries(string name, IEnumerable<TTick> ticks, PeriodOption period)
        {
            Name = name;
            Ticks = (ticks ?? new List<TTick>()).OrderBy(t => t.DateTime).ToList();
            Period = period;

            if (!IsTimeSeriesValid(out var errorTick))
                throw new InvalidTimeFrameException(errorTick.DateTime);
        }

        public string Name { get; private set; }

        public PeriodOption Period { get; private set; }

        /// <summary>
        /// Ticks of the time series
        /// </summary>
        protected IList<TTick> Ticks { get; private set; }

        /// <summary>
        /// Returns TTick instance by index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>TTick instance</returns>
        public TTick this[int index] => Ticks[index];

        /// <summary>
        /// Maximum tick count of the time series, setting it will immediately trim out older ticks
        /// </summary>
        public int? MaxTickCount
        {
            get
            {
                return _maxTickCount;
            }
            set
            {
                _maxTickCount = value;
                if (_maxTickCount.HasValue && Ticks.Count > _maxTickCount.Value)
                    Ticks = Ticks.Skip(Ticks.Count - _maxTickCount.Value).Take(_maxTickCount.Value).ToList();
            }
        }

        public int Count => Ticks.Count;

        public bool IsReadOnly => Ticks.IsReadOnly;

        public void Add(TTick item)
        {
            var periodInstance = Period.CreateInstance();
            if (Ticks.Any())
            {
                var tickEndTime = periodInstance.NextTimestamp(Ticks.Last().DateTime);
                if (item.DateTime < tickEndTime)
                    throw new InvalidTimeFrameException(item.DateTime);

                if (_maxTickCount.HasValue && Ticks.Count >= _maxTickCount.Value)
                    Ticks = Ticks.Skip(Ticks.Count - _maxTickCount.Value + 1).Take(_maxTickCount.Value).ToList();
            }
            Ticks.Add(item);
        }

        public void Clear() => Ticks.Clear();

        public bool Contains(TTick item) => Ticks.Contains(item);

        public void CopyTo(TTick[] array, int arrayIndex) => Ticks.CopyTo(array, arrayIndex);

        public bool Remove(TTick item) => throw new InvalidOperationException("Remove is not allowed for the time series");

        public IEnumerator<TTick> GetEnumerator() => Ticks.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Ticks.GetEnumerator();

        /// <summary>
        /// Validate if the time series is valid (i.e. is there any overlapping between ticks?)
        /// </summary>
        /// <param name="errorTick">Tick that is overlapped by another</param>
        /// <returns>Returns if the time series is valid</returns>
        private bool IsTimeSeriesValid(out TTick errorTick)
        {
            var periodInstance = Period.CreateInstance();
            errorTick = default(TTick);

            for (int i = 0; i < Ticks.Count() - 1; i++)
            {
                var candleEndTime = periodInstance.NextTimestamp(Ticks.ElementAt(i).DateTime);
                if (candleEndTime > Ticks.ElementAt(i + 1).DateTime)
                {
                    errorTick = Ticks.ElementAt(i);
                    return false;
                }
            }
            return true;
        }
    }
}