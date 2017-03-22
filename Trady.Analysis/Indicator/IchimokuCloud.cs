using System;
using Trady.Core;
using Trady.Core.Helper;
using Trady.Core.Period;
using static Trady.Analysis.Indicator.IchimokuCloud;

namespace Trady.Analysis.Indicator
{
    public partial class IchimokuCloud : IndicatorBase<IndicatorResult>
    {
        private LowestLow _shortLowestLow, _middleLowestLow;
        private Func<int, decimal?> _conversionLine, _baseLine, _leadingSpanB;
        private IPeriod _periodInstance;

        public IchimokuCloud(Equity equity, int shortPeriodCount, int middlePeriodCount, int longPeriodCount)
            : base(equity, shortPeriodCount, middlePeriodCount, longPeriodCount)
        {
            // Default country set to United States Of America
            _periodInstance = equity.Period.CreateInstance(Country.UnitedStatesOfAmerica);

            var shortHighestHigh = new HighestHigh(equity, shortPeriodCount);
            _shortLowestLow = new LowestLow(equity, shortPeriodCount);
            _conversionLine = i => (shortHighestHigh.ComputeByIndex(i).HighestHigh + _shortLowestLow.ComputeByIndex(i).LowestLow) / 2;

            var middleHighestHigh = new HighestHigh(equity, middlePeriodCount);
            _middleLowestLow = new LowestLow(equity, middlePeriodCount);
            _baseLine = i => (middleHighestHigh.ComputeByIndex(i).HighestHigh + _middleLowestLow.ComputeByIndex(i).LowestLow) / 2;

            var longHighestHigh = new HighestHigh(equity, longPeriodCount);
            var longLowestLow = new LowestLow(equity, longPeriodCount);
            _leadingSpanB = i => (longHighestHigh.ComputeByIndex(i).HighestHigh + longLowestLow.ComputeByIndex(i).LowestLow) / 2;
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
            bool dataInRange = index >= 0 && index < Equity.Count;
            var conversionLine = dataInRange ? _conversionLine(index) : null;
            var baseLine = dataInRange ? _baseLine(index) : null;

            // Leading
            bool dataAfterMiddlePeriodCount = index >= MiddlePeriodCount;
            var leadingSpanA = dataAfterMiddlePeriodCount ? (_conversionLine(index - MiddlePeriodCount) + _baseLine(index - MiddlePeriodCount)) / 2 : null;
            var leadingSpanB = dataAfterMiddlePeriodCount ? _leadingSpanB(index - MiddlePeriodCount) : null;

            // Lagging
            bool dataBeforeEquityCountMinusMiddlePeriodCount = index <= Equity.Count - MiddlePeriodCount;
            var laggingSpan = dataBeforeEquityCountMinusMiddlePeriodCount ? Equity[index + MiddlePeriodCount - 1].Close : (decimal?)null;

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
            => base.ComputeEndIndex(endTime) + MiddlePeriodCount;
    }
}