using System;
using Trady.Core;
using Trady.Core.Helper;
using Trady.Core.Period;
using static Trady.Analysis.Indicator.IchimokuCloud;

namespace Trady.Analysis.Indicator
{
    public partial class IchimokuCloud : IndicatorBase<IndicatorResult>
    {
        private HighestHigh _shortHighestHigh, _middleHighestHigh, _longHighestHigh;
        private LowestLow _shortLowestLow, _middleLowestLow, _longLowestLow;
        private Func<int, decimal?> _conversionLine, _baseLine, _leadingSpanB;
        private IPeriod _periodInstance;

        public IchimokuCloud(Equity equity, int shortPeriodCount, int middlePeriodCount, int longPeriodCount)
            : base(equity, shortPeriodCount, middlePeriodCount, longPeriodCount)
        {
            // Default country set to United States Of America
            _periodInstance = equity.Period.CreateInstance(Country.UnitedStatesOfAmerica);

            _shortHighestHigh = new HighestHigh(equity, shortPeriodCount);
            _shortLowestLow = new LowestLow(equity, shortPeriodCount);
            _conversionLine = i => (_shortHighestHigh.ComputeByIndex(i).HighestHigh + _shortLowestLow.ComputeByIndex(i).LowestLow) / 2;

            _middleHighestHigh = new HighestHigh(equity, middlePeriodCount);
            _middleLowestLow = new LowestLow(equity, middlePeriodCount);
            _baseLine = i => (_middleHighestHigh.ComputeByIndex(i).HighestHigh + _middleLowestLow.ComputeByIndex(i).LowestLow) / 2;

            _longHighestHigh = new HighestHigh(equity, longPeriodCount);
            _longLowestLow = new LowestLow(equity, longPeriodCount);
            _leadingSpanB = i => (_longHighestHigh.ComputeByIndex(i).HighestHigh + _longLowestLow.ComputeByIndex(i).LowestLow) / 2;

            RegisterDependencies(_shortHighestHigh, _shortLowestLow, _middleHighestHigh, _middleLowestLow, _longHighestHigh, _longLowestLow);
        }

        public void InitWithCountry(Country country)
        {
            _periodInstance = Equity.Period.CreateInstance(country);
        }

        public int ShortPeriodCount => Parameters[0];

        public int MiddlePeriodCount => Parameters[1];

        public int LongPeriodCount => Parameters[2];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            // Current
            var conversionLine = (index >= 0 && index < Equity.Count) ? _conversionLine(index) : null;
            var baseLine = (index >= 0 && index < Equity.Count) ? _baseLine(index) : null;

            // Leading
            var leadingSpanA = (index >= MiddlePeriodCount - 1) ? (_conversionLine(index - MiddlePeriodCount + 1) + _baseLine(index - MiddlePeriodCount + 1)) / 2 : null;
            var leadingSpanB = (index >= MiddlePeriodCount - 1) ? _leadingSpanB(index - MiddlePeriodCount + 1) : null;

            // Lagging
            var laggingSpan = (index + MiddlePeriodCount <= Equity.Count) ? Equity[index + MiddlePeriodCount - 1].Close : (decimal?)null;

            return new IndicatorResult(GetDateTimeByIndex(index), conversionLine, baseLine, leadingSpanA, leadingSpanB, laggingSpan);
        }

        private DateTime GetDateTimeByIndex(int index)
        {
            DateTime dateTime;
            if (index < 0)
            {
                if (_periodInstance is IIntradayPeriod)
                    dateTime = _periodInstance.TimestampAt(Equity[0].DateTime, index);
                else
                {
                    try
                    {
                        dateTime = ((IInterdayPeriod)_periodInstance).BusinessTimestampAtAsync(Equity[0].DateTime, index).Result;
                    }
                    catch
                    {
                        // Unable to get business datetime from enrico web service, use -1 directly in this case
                        dateTime = _periodInstance.TimestampAt(Equity[0].DateTime, index);
                    }
                }
            }
            else if (index >= Equity.Count)
            {
                if (_periodInstance is IIntradayPeriod)
                    dateTime = _periodInstance.TimestampAt(Equity[Equity.Count - 1].DateTime, index - Equity.Count + 1);
                else
                {
                    try
                    {
                        dateTime = ((IInterdayPeriod)_periodInstance).BusinessTimestampAtAsync(Equity[Equity.Count - 1].DateTime, index - Equity.Count + 1).Result;
                    }
                    catch
                    {
                        // Unable to get business datetime from enrico web service, use +1 directly in this case
                        dateTime = _periodInstance.TimestampAt(Equity[Equity.Count - 1].DateTime, index - Equity.Count + 1);
                    }
                }
            }
            else
                dateTime = Equity[index].DateTime;
            return dateTime;
        }

        protected override int ComputeStartIndex(DateTime? startTime)
            => base.ComputeStartIndex(startTime) - MiddlePeriodCount + 1;

        protected override int ComputeEndIndex(DateTime? endTime)
            => base.ComputeEndIndex(endTime) + MiddlePeriodCount - 1;
    }
}