using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;
using Trady.Core.Helper;
using static Trady.Analysis.Indicator.IchimokuCloud;

namespace Trady.Analysis.Indicator
{
    public partial class IchimokuCloud : IndicatorBase<IndicatorResult>
    {
        private HighestHigh _shortHighestHigh, _middleHighestHigh, _longHighestHigh;
        private LowestLow _shortLowestLow, _middleLowestLow, _longLowestLow;
        private Func<int, decimal?> _conversionLine, _baseLine, _leadingSpanB;

        public IchimokuCloud(Equity equity, int shortPeriodCount, int middlePeriodCount, int longPeriodCount) 
            : base(equity, shortPeriodCount, middlePeriodCount, longPeriodCount)
        {
            _shortHighestHigh = new HighestHigh(equity, shortPeriodCount);
            _shortLowestLow = new LowestLow(equity, shortPeriodCount);
            _conversionLine = i => (_shortHighestHigh.ComputeByIndex(i).HighestHigh + _shortLowestLow.ComputeByIndex(i).LowestLow) / 2;

            _middleHighestHigh = new HighestHigh(equity, middlePeriodCount);
            _middleLowestLow = new LowestLow(equity, middlePeriodCount);
            _baseLine = i => (_middleHighestHigh.ComputeByIndex(i).HighestHigh + _middleLowestLow.ComputeByIndex(i).LowestLow) / 2;

            _longHighestHigh = new HighestHigh(equity, longPeriodCount);
            _longLowestLow = new LowestLow(equity, longPeriodCount);
            _leadingSpanB = i => (_longHighestHigh.ComputeByIndex(i).HighestHigh + _longLowestLow.ComputeByIndex(i).LowestLow) / 2;
        }

        public int ShortPeriodCount => Parameters[0];

        public int MiddlePeriodCount => Parameters[1];

        public int LongPeriodCount => Parameters[2];

        public override IndicatorResult ComputeByIndex(int index)
        {
            // Current
            var conversionLine = index < Equity.Count ? _conversionLine(index) : null;
            var baseLine = index < Equity.Count ? _baseLine(index) : null;

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
                dateTime = Equity.Period.CreateInstance().TimestampAt(Equity[0].DateTime, index);
            else if (index >= Equity.Count)
                dateTime = Equity.Period.CreateInstance().TimestampAt(Equity[Equity.Count - 1].DateTime, index - Equity.Count + 1);
            else
                dateTime = Equity[index].DateTime;
            return dateTime;
        }

        protected override int GetStartIndex(DateTime? startTime)
            => base.GetStartIndex(startTime) - MiddlePeriodCount + 1;

        protected override int GetEndIndex(DateTime? endTime)
            => base.GetEndIndex(endTime) + MiddlePeriodCount - 1;
    }
}
